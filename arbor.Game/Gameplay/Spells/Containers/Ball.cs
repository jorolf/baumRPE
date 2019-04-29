using arbor.Game.Worlds;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace arbor.Game.Gameplay.Spells.Containers
{
    public class Ball : SpellContainer
    {
        public Ball(World world, SpellType type, params ISpellAttribute[] attributes)
            : base(world, type, attributes)
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
                if (draw is ArborCharacter)
                {
                    ArborCharacter character = draw as ArborCharacter;
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
