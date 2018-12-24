using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace arbor.Game.Gameplay
{
    public class ArborUI : Container
    {
        public SpriteText Mana;
        public SpriteText Health;

        public ArborUI()
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
