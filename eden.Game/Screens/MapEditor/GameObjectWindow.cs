using System.Collections.Generic;
using System.Linq;
using eden.Game.Gameplay;
using eden.Game.Graphics.Containers;
using eden.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace eden.Game.Screens.MapEditor
{
    public class GameObjectWindow : EdenWindow
    {
        private Dropdown<Tile> tileDropdown;

        private GameObject gameObject;

        public GameObject GameObject
        {
            get => gameObject;
            set => gameObject = value;
        }

        private TileAtlas tileAtlas;

        public TileAtlas TileAtlas
        {
            get => tileAtlas;
            set
            {
                if (value == tileAtlas) return;

                tileAtlas = value;

                tileDropdown.Items = tileAtlas.Select((tile, i) => new KeyValuePair<string, Tile>($"{i}: {tile.GetTexture(0).AssetName}", tile));
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = new FillFlowContainer
            {
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.Y,
                Width = 200,
                Children = new Drawable[]
                {
                    tileDropdown = new BasicDropdown<Tile>(),
                    new Button
                    {
                        Text = "Remove",
                        RelativeSizeAxes = Axes.X,
                        Height = 30,
                    }
                }
            };
        }
    }
}
