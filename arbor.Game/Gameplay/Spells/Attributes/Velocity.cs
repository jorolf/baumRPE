using osuTK;

namespace arbor.Game.Gameplay.Spells.Attributes
{
    public class Velocity : ISpellAttribute
    {
        public Vector2 SpellVelocity;

        public void Init(SpellContainer container)
        {
        }

        public void Update(SpellContainer container)
        {
            container.Position += SpellVelocity * (float)container.Time.Elapsed;
        }
    }
}
