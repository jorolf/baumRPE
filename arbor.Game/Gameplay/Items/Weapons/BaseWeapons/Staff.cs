using arbor.Game.Gameplay.Items.Weapons.Type;

namespace arbor.Game.Gameplay.Items.Weapons.BaseWeapons
{
    public abstract class Staff : Melee
    {
        /// <summary>
        /// The total distance the stab will cover
        /// </summary>
        public float StabLength { get; set; }

        protected Staff(Character character) : base(character)
        {
        }
    }
}
