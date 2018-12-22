namespace eden.Game.Gameplay.Spells.Attributes
{
    public class Duration : ISpellAttribute
    {
        public double SpellDuration;

        private double spellStartTime;

        public void Init(SpellContainer container)
        {
            spellStartTime = container.Time.Current;
        }

        public void Update(SpellContainer container)
        {
            if (container.Time.Current - spellStartTime > SpellDuration)
                container.Expire();
        }
    }
}
