using System;
using System.Linq;
using arbor.Game.Graphics.UserInterface;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace arbor.Game.Screens.MapEditor
{
    public class Toolbar : OverlayContainer
    {
        public Action ShowOpenWorld;
        public Action ShowCreateWorld;
        public Action ShowCreateTileAtlas;
        public Action ResetView;
        public Action Exit;

        public Action OpenStoryFolder;
        public Action ReloadResources;
        public Action ShowStoryOptions;

        public Action ShowResizeWorld;
        public Action ShowRenameWorld;
        public Action ShowWorldOptions;

        public readonly Bindable<MapEditorScreen.EditorMode> CurrentMode = new Bindable<MapEditorScreen.EditorMode>();

        public Toolbar()
        {
            State = Visibility.Visible;
            Depth = -float.MaxValue;

            ArborList modeList;
            Add(new FillFlowContainer
            {
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Children = new[]
                {
                    new ArborMenu(Direction.Horizontal, true)
                    {
                        Depth = -1,
                        Height = 30,
                        RelativeSizeAxes = Axes.X,
                        Items = new[]
                        {
                            new MenuItem("File")
                            {
                                Items = new[]
                                {
                                    new MenuItem("Open World", () => ShowOpenWorld()),
                                    new MenuItem("Create World", () => ShowCreateWorld()),
                                    new MenuItem("Create Tile Atlas", () => ShowCreateTileAtlas()),
                                    new MenuItem("Reset View", () => ResetView()),
                                    new MenuItem("Save & Exit", () => Exit()),
                                }
                            },
                            new MenuItem("Story")
                            {
                                Items = new[]
                                {
                                    new MenuItem("Open Story Folder", () => OpenStoryFolder()),
                                    new MenuItem("Reload Resources", () => ReloadResources()),
                                    new MenuItem("Story Options", () => ShowStoryOptions()),
                                }
                            },
                            new MenuItem("World")
                            {
                                Items = new[]
                                {
                                    new MenuItem("Resize World", () => ShowResizeWorld()),
                                    new MenuItem("Rename World", () => ShowRenameWorld()),
                                    new MenuItem("World Options", () => ShowWorldOptions()),
                                }
                            },
                            new MenuItem("View")
                            {
                                Items = new MenuItem[]
                                {
                                }
                            },
                        }
                    },
                    modeList = new ArborList(Direction.Horizontal)
                    {
                        Height = 30,
                        RelativeSizeAxes = Axes.X,
                        Items = Enum.GetValues(typeof(MapEditorScreen.EditorMode)).Cast<MapEditorScreen.EditorMode>().Select(mode => new MenuItem(mode.ToString())).ToArray()
                    }
                }
            });

            modeList.Current.ValueChanged += item => CurrentMode.Value = Enum.TryParse(item.Text, out MapEditorScreen.EditorMode mode) ? mode : CurrentMode.Default;
            CurrentMode.ValueChanged += mode => modeList.Current.Value = modeList.Items.First(item => item.Text == mode.ToString());
            CurrentMode.TriggerChange();
        }

        protected override void PopIn() => Alpha = 1;

        protected override void PopOut() => Alpha = 0;
    }
}
