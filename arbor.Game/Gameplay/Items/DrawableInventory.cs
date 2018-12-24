using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace arbor.Game.Gameplay.Items
{
    public class DrawableInventory : Container
    {
        public Inventory Inventory
        {
            get { return inventory; }
            set
            {
                if (inventory != value)
                {
                    inventory = value;

                    itemList = new FillFlowContainer<ListItem>
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                    };

                    foreach (Item item in value.CombinedItemList)
                        itemList.Add(new ListItem(item));
                }
            }
        }

        private Inventory inventory;

        private FillFlowContainer<ListItem> itemList;

        public DrawableInventory()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Alpha = 0;
            AlwaysPresent = true;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.8f,
                    Colour = Color4.Black
                },
                new SpriteText
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    TextSize = 10,
                    Text = "Inventory"
                },
                new ScrollContainer
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.Both,
                    Height = 0.9f,

                    Child = itemList = new FillFlowContainer<ListItem>
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                    }
                }
            };
        }

        private class ListItem : Container
        {
            public readonly Item Item;

            public ListItem(Item item)
            {
                Item = item;

                Children = new Drawable[]
                {
                    new Sprite
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Size = new Vector2(10),
                        Texture = Item.ItemTexture
                    }
                };
            }
        }
    }
}
