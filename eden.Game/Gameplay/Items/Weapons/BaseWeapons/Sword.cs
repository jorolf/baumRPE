using eden.Game.Gameplay.Items.Weapons.Type;
using osu.Framework.Graphics;

namespace eden.Game.Gameplay.Items.Weapons.BaseWeapons
{
    public abstract class Sword : Melee
    {
        /// <summary>
        /// The angle that this sweep will cover (Degrees)
        /// </summary>
        public float SweepWidth { get; set; } = 90;

        /// <summary>
        /// The angle that this sweep will cover (Degrees)
        /// </summary>
        public float SweepOffset { get; set; }

        /// <summary>
        /// The easing this swing animation will use
        /// </summary>
        public Easing SwingEasing { get; } = Easing.None;

        protected Sword(Character character) : base(character)
        {
        }

        protected override void Update()
        {
            base.Update();

            Position = ParentCharacter.Position;
        }

        public override void Attack() => Swing();

        public void Swing()
        {
            Rotation = ParentCharacter.Rotation + SweepOffset - SweepWidth / 2;
            Alpha = 1;
            this.RotateTo(Rotation + SweepWidth, AttackSpeed, SwingEasing)
                .Delay(AttackSpeed)
                .FadeOut();
        }
    }
}
