using osuTK.Graphics;

namespace eden.Game.Gameplay.Spells.Types
{
    public class Fire : SpellType
    {
        public override Color4 GetColor() => Color4.OrangeRed;

        public override string GetName() => "fire";
    }
}
