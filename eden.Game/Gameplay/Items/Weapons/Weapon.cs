namespace eden.Game.Gameplay.Items.Weapons
{
    public abstract class Weapon : Item
    {
        public abstract float Damage { get; }

        /// <summary>
        /// The Time it takes for the attack animation in miliseconds (lower => faster attack)
        /// </summary>
        public float AttackSpeed { get; set; } = 500;

        /// <summary>
        /// The owner of this Weapon
        /// </summary>
        public readonly Character ParentCharacter;

        protected Weapon(Character character) : base(ItemType.Weapon)
        {
            ParentCharacter = character;
        }

        public virtual void Attack()
        {
        }

        public virtual void Hit(Character character)
        {
        }
    }
}
