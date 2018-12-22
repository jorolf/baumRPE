namespace eden.Game.Gameplay.Characters.Friends
{
    /// <summary>
    /// Wield magic, disliked by "NonMages"
    /// </summary>
    public abstract class Mage : Character
    {
        /// <summary>
        /// Serves as a multiplier based on how powerfull this specific mage is
        /// </summary>
        public abstract float MageSkill { get; }

        //private float SpellReady;
        /*
        public CastSpell(Spell spell)
        {
            SpellReady = (Time.Current + spell.SpellCooldown) * MageSkill;

            //Add spell to world here
        }
        */
    }
}
