namespace eden.Game.Gameplay.Spells
{
    public interface ISpellAttribute
    {
        void Init(SpellContainer container);
        void Update(SpellContainer container);
    }
}
