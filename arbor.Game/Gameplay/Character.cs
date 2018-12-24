using System;

namespace arbor.Game.Gameplay
{
    /// <summary>
    /// Base Character class, if it seems like a property all characters should have drop it here
    /// </summary>
    public abstract class Character : GameObject
    {
        /// <summary>
        /// Maximum health this charcter can have
        /// </summary>
        public abstract float MaxHealth { get; }

        /// <summary>
        /// The team this character is on, used mostly for Hitbox
        /// </summary>
        public abstract int Team { get; }

        /// <summary>
        /// If this character has hit 0 health
        /// </summary>
        public bool Dead;

        private float health;

        /// <summary>
        /// the amount of health this character has
        /// </summary>
        public float Health
        {
            get => health;
            set
            {
                health = value;
                if (health < 0)
                    Death();
            }
        }

        protected Character()
        {
            Health = MaxHealth;
        }

        //protected override void LoadComplete()
        //{
        //    base.LoadComplete();
        //    //TODO: move this to ctor somehow
        //    //Hitbox.Team = Team;
        //}

        /// <summary>
        /// Removes "damage"
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public float Hit(float damage)
        {
            return Health = Math.Max(0, Health - damage);
        }

        /// <summary>
        /// Adds "health"
        /// </summary>
        /// <param name="health"></param>
        /// <returns></returns>
        public float Heal(float health)
        {
            return Health = Math.Min(MaxHealth, Health + health);
        }

        /// <summary>
        /// Called when this character runs out of health
        /// </summary>
        public virtual void Death()
        {
            Dead = true;
            Expire();
        }
    }
}
