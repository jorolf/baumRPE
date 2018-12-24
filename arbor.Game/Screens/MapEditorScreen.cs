using System;
using System.IO;
using System.Linq;
using arbor.Game.Gameplay;
using arbor.Game.IO;
using arbor.Game.Screens.MapEditor;
using arbor.Game.Screens.MapEditor.MapWindows;
using arbor.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.IO.File;
using osu.Framework.Platform;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace arbor.Game.Screens
{
    public class MapEditorScreen : ArborScreen
    {
        private Storage storage;
        private Story story;
        private DragableWorld worldContainer;
        private TileAtlasWindow tileAtlasWindow;

        private World worldField;

        private World world
        {
            get => worldField;
            set
            {
                if (value == worldField) return;

                worldField?.Save();
                worldField = value;

                worldContainer.World = value;

                tileAtlasWindow.TileAtlas = value.Atlas;
            }
        }

        private readonly Bindable<EditorMode> mode = new Bindable<EditorMode>
        {
            Default = EditorMode.Tiles,
        };

        [BackgroundDependencyLoader]
        private void load(Story story, ArborGame game, JsonStore jsonStore)
        {
            this.story = story;
            storage = game.StoryStorage;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Gray,
                },
                worldContainer = new DragableWorld
                {
                    RelativeSizeAxes = Axes.Both,
                }
            };

            var newTileAtlasWindow = addWindow<string, NewTileAtlasWindow>(filename =>
            {
                using (var writer = new StreamWriter(storage.GetStream(filename, FileAccess.Write)))
                    writer.Write("[]");

                story.AtlasFiles.Add(filename);
            });

            var newWorldWindow = addWindow<World, NewWorldWindow>(world =>
            {
                this.world = world;
                if (story.SpawnWorldFile == null)
                    story.SpawnWorldFile = world.WorldName;
            });
            newWorldWindow.NewTileAtlasWindow = newTileAtlasWindow;

            var openWorldWindow = addWindow<World, OpenWorldWindow>(world => this.world = world);

            var resizeWorldWindow = addWindow<Vector2I, ResizeWorldWindow>(vec => world.ResizeWorld(vec.X, vec.Y));

            var worldOptionsWindow = addWindow<Action<World>, WorldOptionsWindow>(action => action(world));

            var storyOptionsWindow = addWindow<Action, StoryOptionsWindow>(action => action());

            var renameWorldWindow = addWindow<string, RenameWorldWindow>(newName =>
            {
                string oldName = worldField.WorldName;
                worldField.WorldName = newName;
                worldField.Save();
                storage.Delete(oldName);
                story.WorldFiles.Remove(oldName);
                story.WorldFiles.Add(newName);
                if (story.SpawnWorldFile == oldName)
                    story.SpawnWorldFile = newName;

                jsonStore.Serialize(Story.FILENAME, story);
            });

            Add(tileAtlasWindow = new TileAtlasWindow
            {
                State = Visibility.Visible,
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft
            });

            Exited += _ =>
            {
                jsonStore.Serialize(Story.FILENAME, story);
                worldField.Save();
            };

            Toolbar toolbar;
            Add(toolbar = new Toolbar
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                ShowCreateTileAtlas = newTileAtlasWindow.Show,
                ShowCreateWorld = newWorldWindow.Show,
                ShowOpenWorld = openWorldWindow.Show,
                OpenStoryFolder = () => storage.OpenInNativeExplorer(),
                ShowResizeWorld = resizeWorldWindow.Show,
                ShowWorldOptions = worldOptionsWindow.Show,
                ShowRenameWorld = renameWorldWindow.Show,
                ReloadResources = reloadResources,
                ResetView = worldContainer.ResetView,
                ShowStoryOptions = storyOptionsWindow.Show,
                Exit = Exit,
            });
            toolbar.CurrentMode.BindTo(mode);

            if (story.SpawnWorldFile == null)
                newWorldWindow.Show();
            else
            {
                if (!storage.Exists(story.SpawnWorldFile)) return;

                world = jsonStore.Deserialize<World>(story.SpawnWorldFile);
                world.WorldName = story.SpawnWorldFile;
            }

            mode.ValueChanged += newValue =>
            {
                switch (newValue)
                {
                    case EditorMode.Tiles:
                        tileAtlasWindow.Show();
                        break;
                    case EditorMode.Objects:
                        tileAtlasWindow.Hide();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(newValue), newValue, null);
                }
            };
        }

        private TMapWindow addWindow<T, TMapWindow>(Action<T> onSubmit)
            where TMapWindow : MapWindow<T>, new()
        {
            TMapWindow window;
            Add(window = new TMapWindow
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });
            window.OnSubmit += onSubmit;
            return window;
        }

        protected override bool OnMouseDown(MouseDownEvent mouseDownEvent)
        {
            switch ((EditorMode)mode)
            {
                case EditorMode.Tiles:
                    return mouseDownEvent.Button == MouseButton.Left && placeTileAt(mouseDownEvent.MousePosition);
                case EditorMode.Objects:
                    if (worldContainer.GetTileAt(mouseDownEvent.ScreenSpaceMouseDownPosition) is Vector2I objectPos)
                    {
                        var chunk = worldContainer.World.GetChunk(objectPos.X / Chunk.TILES_PER_CHUNK, objectPos.Y / Chunk.TILES_PER_CHUNK);
                        var gameObject = chunk.GetObjectAt(objectPos);
                        if (gameObject == null)
                        {
                            gameObject = new GameObject { WorldPosition = objectPos };
                            chunk.AddObject(gameObject);
                        }


                        return true;
                    } else
                        return base.OnMouseDown(mouseDownEvent);
                default:
                    return base.OnMouseDown(mouseDownEvent);
            }
        }

        protected override bool OnMouseMove(MouseMoveEvent mouseMoveEvent)
        {
            switch ((EditorMode)mode)
            {
                case EditorMode.Tiles:
                    return mouseMoveEvent.IsPressed(MouseButton.Left) && placeTileAt(mouseMoveEvent.ScreenSpaceMousePosition);
                default:
                    return base.OnMouseMove(mouseMoveEvent);
            }
        }

        private bool placeTileAt(Vector2 screenSpaceMousePos)
        {
            Vector2I? tilePosNull = worldContainer.GetTileAt(screenSpaceMousePos);

            if (!tilePosNull.HasValue) return false;
            Vector2I tilePos = tilePosNull.Value;

            int index = tileAtlasWindow.SelectedTileIndex;
            if(index != -1)
                world.GetChunk(tilePos.X / Chunk.TILES_PER_CHUNK, tilePos.Y / Chunk.TILES_PER_CHUNK).Tiles[tilePos.X % Chunk.TILES_PER_CHUNK, tilePos.Y % Chunk.TILES_PER_CHUNK] = tileAtlasWindow.SelectedTileIndex;

            return true;
        }

        private void reloadResources()
        {
            story.AtlasFiles.Clear();
            story.WorldFiles.Clear();
            story.ResourceFiles.Clear();
            reloadDirectory("");

            if (!story.WorldFiles.Contains(story.SpawnWorldFile))
                story.SpawnWorldFile = story.WorldFiles.FirstOrDefault();
        }

        private static readonly string[] ignored_dirs =  { "obj", "bin" };

        private void reloadFile(string file)
        {

            switch (FileSafety.PathStandardise(file))
            {
                case var f when f.EndsWith(".atlas"):
                    story.AtlasFiles.Add(f);
                    break;
                case var f when f.EndsWith(".world"):
                    story.WorldFiles.Add(f);
                    break;
                case var f when f.EndsWith(".png") || f.EndsWith(".jpg") || f.EndsWith(".jpeg"):
                    story.ResourceFiles.Add(f);
                    break;
            }
        }

        private void reloadDirectory(string dir)
        {
            if (ignored_dirs.Contains(dir)) return;

            storage.GetFiles(dir).ForEach(reloadFile);
            storage.GetDirectories(dir).ForEach(reloadDirectory);
        }

        public enum EditorMode
        {
            Tiles,
            Objects,
        }
    }
}
