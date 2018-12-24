using System.Linq;
using arbor.Game.Gameplay;
using arbor.Game.Graphics.Containers;
using arbor.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace arbor.Game.Screens.MapEditor
{
    public class GameObjectWindow : ArborWindow
    {
        private Dropdown<string> tileDropdown;

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

                tileDropdown.Items = tileAtlas.Select((tile, i) => $"{i}: {tile.GetTexture(0).AssetName}");
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
                    tileDropdown = new BasicDropdown<string>(),
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
