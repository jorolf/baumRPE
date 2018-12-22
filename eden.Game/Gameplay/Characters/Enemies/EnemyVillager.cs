using System;

namespace eden.Game.Gameplay.Characters.Enemies
{
    /// <summary>
    /// "Enemy" in this case does not mean inheritly bad, just means if they find out your a mage you will be reported to the "authorities"
    /// </summary>
    public class EnemyVillager : NonMage
    {
        public override float MaxHealth
        {
            get { throw new NotImplementedException(); }
        }

        public override int Team
        {
            get { throw new NotImplementedException(); }
        }
    }
}
