using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using OpenTK.Graphics;

namespace eden.Game.Gameplay
{
    public class EdenUI : Container
    {
        public SpriteText Mana;
        public SpriteText Health;

        public EdenUI()
        {
            Children = new Drawable[]
            {
                Health = new SpriteText
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Text = "Health Value Here",
                    TextSize = 40,
                    Colour = Color4.Red,
                },
                Mana = new SpriteText
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Text = "Mana Values Here",
                    TextSize = 40,
                    Colour = Color4.Blue,
                }
            };
        }
    }
}
