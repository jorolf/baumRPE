using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using eden.Game.Graphics.Containers;
using eden.Game.Graphics.UserInterface;
using eden.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;

namespace eden.Game.Screens.MapEditor
{
    public class TileAtlasWindow : EdenWindow, IHasCurrentValue<Tile>
    {
        private TileAtlas tileAtlas;
        private readonly FillFlowContainer<ClickableTile> drawableTiles;
        private FillFlowContainer tileProperties;

        private readonly Tile addTile = new StaticTile
        {
            File = "Editor/AddTile"
        };

        /// <summary>
        /// Return -1 if no tile or a new tile is selected
        /// </summary>
        public int SelectedTileIndex => tileAtlas.IndexOf(Current);

        public Tile SelectedTile
        {
            set => Current.Value = value;
            get => Current;
        }

        public TileAtlas TileAtlas
        {
            get => tileAtlas;
            set
            {
                if (value == tileAtlas) return;

                tileAtlas = value;
                Content.Alpha = tileAtlas == null ? 0 : 1;

                updateTiles();
                SelectedTile = null;
                Title = $"Tile atlas ({tileAtlas.Filename})";
            }
        }

        public TileAtlasWindow()
        {
            Closeable = false;
            AddRange(new Drawable[]
            {
                new ScrollContainer
                {
                    Size = new Vector2(300, 375),
                    Child = drawableTiles = new FillFlowContainer<ClickableTile>
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y
                    }
                }
            });

            Current.ValueChanged += updateSelectedTile;
            TileAtlas = null;
        }

        private void updateSelectedTile(Tile tile)
        {
            if (tileProperties != null)
                Remove(tileProperties);

            switch (tile)
            {
                case StaticTile staticTile:
                    Add(tileProperties = new StaticTileProperties(staticTile)
                    {
                        Atlas = tileAtlas,
                        Margin = new MarginPadding { Top = 375 },
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        ChangeType = changeType,
                        DeleteTile = deleteTile,
                    });
                    break;
                case AnimatedTile animatedTile:
                    Add(tileProperties = new AnimatedTileProperties(animatedTile)
                    {
                        Atlas = tileAtlas,
                        Margin = new MarginPadding { Top = 375 },
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        ChangeType = changeType,
                        DeleteTile = deleteTile,
                    });
                    break;
            }
        }

        private void deleteTile(Tile tile)
        {
            tileAtlas.Remove(tile);
            Remove(tileProperties);
            tileAtlas.Save();
            updateTiles();
        }

        private void changeType(Tile tile)
        {
            tile.Solid = SelectedTile.Solid;
            switch (tile)
            {
                case AnimatedTile animatedTile:
                    animatedTile.Frames.Add(((StaticTile)SelectedTile).File);
                    animatedTile.Speed = 1000f;
                    break;
                case StaticTile staticTile:
                    staticTile.File = ((AnimatedTile)SelectedTile).Frames[0];
                    break;
            }

            tileAtlas[tileAtlas.IndexOf(SelectedTile)] = tile;
            updateTiles();
            SelectedTile = tile;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            addTile.LoadTextures(textures);
        }

        private void updateTiles()
        {
            drawableTiles.Clear();
            foreach (var tile in tileAtlas)
            {
                drawableTiles.Add(new ClickableTile(tile, Current)
                {
                    Action = () => SelectedTile = tile,
                });
            }

            drawableTiles.Add(new ClickableTile(addTile, Current)
            {
                Action = addNewTile
            });
        }

        private void addNewTile()
        {
            Tile tile = new StaticTile();
            tileAtlas.Add(tile);
            updateTiles();
            SelectedTile = tile;
        }

        private class ClickableTile : ClickableContainer
        {
            private readonly Box hoverBox;

            public ClickableTile(Tile tile, Bindable<Tile> selectedTile)
            {
                AutoSizeAxes = Axes.Both;
                BorderColour = Color4.Black;
                Masking = true;

                Children = new Drawable[]
                {
                    new DrawableTile(tile)
                    {
                        Size = new Vector2(75),
                    },
                    hoverBox = new Box
                    {
                        Alpha = 0,
                        RelativeSizeAxes = Axes.Both,
                    }
                };

                selectedTile.ValueChanged += sTile => BorderThickness = tile == sTile ? 3 : 0;
            }

            protected override bool OnHover(HoverEvent hoverEvent)
            {
                hoverBox.Alpha = 0.5f;
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent hoverLostEvent)
            {
                hoverBox.Alpha = 0;
                base.OnHoverLost(hoverLostEvent);
            }
        }

        private class TileProperties<T> : FillFlowContainer where T : Tile
        {
            protected readonly T Tile;
            public Action<Tile> ChangeType;
            public Action<Tile> DeleteTile;
            public TileAtlas Atlas;
            private readonly BasicCheckbox solidCheckbox;
            private bool remove;
            protected Story Story;

            protected TileProperties(T tile)
            {
                Tile = tile;
                Direction = FillDirection.Vertical;
                Spacing = new Vector2(0, 5);
                Add(solidCheckbox = new BasicCheckbox
                {
                    LabelText = "Solid",
                });
                solidCheckbox.Current.Value = tile.Solid;

                BasicDropdown<Func<Tile>> dropdown;
                Add(dropdown = new BasicDropdown<Func<Tile>>
                {
                    RelativeSizeAxes = Axes.X,
                    Items = new Dictionary<string, Func<Tile>>
                    {
                        { "Static", () => new StaticTile() },
                        { "Animated", () => new AnimatedTile() }
                    }
                });
                dropdown.Current.Value = dropdown.Items.First(pair => (typeof(T) == typeof(AnimatedTile) ? "Animated" : "Static") == pair.Key).Value;
                dropdown.Current.ValueChanged += type =>
                {
                    Tile.Solid = solidCheckbox.Current;
                    ChangeType(type());
                };

                Button saveButton;
                Add(saveButton = new Button
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 20,
                    Text = "Save",
                    BackgroundColour = Color4.Green,
                    Masking = true,
                    CornerRadius = 5,
                });
                saveButton.Action = () => saveButton.BackgroundColour = Save() ? Color4.Green : Color4.Red;
                SetLayoutPosition(saveButton, 1);

                Button removeButton;
                Add(removeButton = new Button
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 20,
                    Text = "Remove",
                    BackgroundColour = Color4.Red,
                    Masking = true,
                    CornerRadius = 5,
                });
                removeButton.Action = () =>
                {
                    if (remove)
                        DeleteTile(tile);
                    else
                    {
                        removeButton.Text = "Click again to confirm!";
                        remove = true;
                    }
                };
                SetLayoutPosition(removeButton, 1);
            }

            protected virtual bool Save()
            {
                Tile.Solid = solidCheckbox.Current;
                Tile.LoadTextures(Atlas.TextureStore);
                Atlas.Save();

                return true;
            }

            [BackgroundDependencyLoader]
            private void load(Story story)
            {
                Story = story;
            }
        }

        private class StaticTileProperties : TileProperties<StaticTile>
        {
            private Dropdown<string> spriteDropdown;

            public StaticTileProperties(StaticTile tile)
                : base(tile)
            {
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                Add(spriteDropdown = new BasicDropdown<string>
                {
                    RelativeSizeAxes = Axes.X,
                    Items = Story.ResourceFiles.ToDictionary(s => s)
                });
                spriteDropdown.Current.Value = Tile.File;
            }

            protected override bool Save()
            {
                if (string.IsNullOrWhiteSpace(spriteDropdown.Current)) return false;

                Tile.File = spriteDropdown.Current;
                return base.Save();
            }
        }

        private class AnimatedTileProperties : TileProperties<AnimatedTile>
        {
            private TextBox speedTextBox;
            private EdenList list;
            private Dropdown<string> spriteDropdown;
            private List<MenuItem> fileMenus;
            private MenuItem currentFile;

            public AnimatedTileProperties(AnimatedTile tile)
                : base(tile)
            {
            }

            protected override void LoadComplete()
            {
                AddRange(new Drawable[]
                {
                    speedTextBox = new TextBox
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 20,
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Children = new Drawable[]
                        {
                            list = new EdenList(osu.Framework.Graphics.Direction.Vertical)
                            {
                                Items = fileMenus = Tile.Frames.Select(file =>
                                {
                                    MenuItem item = new MenuItem(file);
                                    item.Action.Value = () => select(item);
                                    return item;
                                }).ToList(),
                                RelativeSizeAxes = Axes.X,
                                Width = 0.75f,
                                MaxHeight = 100,
                            },
                            new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                RelativePositionAxes = Axes.X,
                                Width = 0.25f,
                                X = 0.75f,
                                Direction = FillDirection.Vertical,
                                AutoSizeAxes = Axes.Y,
                                Children = new[]
                                {
                                    new Button
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Height = 27,
                                        Text = "Add",
                                        BackgroundColour = Color4.Green,
                                        Action = add,
                                    },
                                    new Button
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Height = 27,
                                        Text = "Remove",
                                        BackgroundColour = Color4.Red,
                                        Action = remove
                                    },
                                }
                            },
                        }
                    },
                    spriteDropdown = new BasicDropdown<string>
                    {
                        RelativeSizeAxes = Axes.X,
                        Items = Story.ResourceFiles.ToDictionary(s => s)
                    }
                });
                speedTextBox.Current.Value = Tile.Speed.ToString(CultureInfo.CurrentCulture);
            }

            private void remove()
            {
                fileMenus.Remove(currentFile);
                currentFile.Text.UnbindBindings();
                spriteDropdown.Current.UnbindBindings();
                spriteDropdown.Current.Value = string.Empty;
                currentFile = null;
                list.Items = fileMenus;
            }

            private void add()
            {
                MenuItem item = new MenuItem(spriteDropdown.Current);
                item.Action.Value = () => select(item);
                fileMenus.Insert(fileMenus.IndexOf(currentFile) + 1, item);


                list.Items = fileMenus;
                select(item);
            }

            private void select(MenuItem item)
            {
                currentFile?.Text.UnbindBindings();
                spriteDropdown.Current.UnbindBindings();
                currentFile = item;
                spriteDropdown.Current.BindTo(currentFile.Text);
                list.Current.Value = item;
            }

            protected override bool Save()
            {
                if (!int.TryParse(speedTextBox.Current, out int speed)) return false;

                var files = new List<string>();
                foreach (MenuItem item in fileMenus)
                {
                    if (string.IsNullOrWhiteSpace(item.Text)) return false;
                    files.Add(item.Text);
                }

                Tile.Frames = files;
                Tile.Speed = speed;
                return base.Save();
            }
        }

        public Bindable<Tile> Current { get; } = new Bindable<Tile>();
    }
}
