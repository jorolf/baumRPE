using arbor.Game.Worlds;

namespace arbor.Game.Gameplay.Spells
{
    public class SpellContainer : GameObject
    {
        public readonly SpellType Type;
        public readonly ISpellAttribute[] Attributes;

        public int Team { get; set; }

        public float Damage { get; set; }

        public SpellContainer(World world, SpellType type, params ISpellAttribute[] attributes)
        {
            Type = type;
            Attributes = attributes;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            foreach (ISpellAttribute attribute in Attributes)
                attribute.Init(this);
        }

        protected override void Update()
        {
            base.Update();

            foreach (ISpellAttribute attribute in Attributes)
                attribute.Update(this);
        }
    }
}
