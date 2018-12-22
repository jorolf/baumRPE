using eden.Game.Worlds;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace eden.Game.Gameplay.Spells.Containers
{
    public class Ball : SpellContainer
    {
        public Ball(World world, SpellType type, params ISpellAttribute[] attributes) : base(world, type, attributes)
        {
            Child = new Circle
            {
                RelativeSizeAxes = Axes.Both,
                Colour = type.GetColor(),
            };
        }

        /*protected override void Update()
        {
            base.Update();

            foreach (Drawable draw in World.Children)
                if (draw is EdenCharacter)
                {
                    EdenCharacter character = draw as EdenCharacter;
                    if (character.Team != Team)
                        if (Hitbox.HitDetect(Hitbox, character.Hitbox))
                        {
                            character.Hit(Damage);
                            Expire();
                        }
                }
        }*/
    }
}
