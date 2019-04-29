using arbor.Game.Gameplay.Items.Weapons.Type;

namespace arbor.Game.Gameplay.Items.Weapons.BaseWeapons
{
    public abstract class Bow : Ranged
    {
        protected Bow(Character character)
            : base(character)
        {
        }
    }
}
