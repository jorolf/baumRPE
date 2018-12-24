using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace arbor.Game.Gameplay.Characters.Enemies
{
    public class Knight : NonMage
    {
        public override float MaxHealth => 40;

        public override int Team => 2;

        private readonly Player player;

        public Knight(Player player)
        {
            this.player = player;

            Children = new Drawable[]
            {
                new Container
                {
                    Size = new Vector2(24),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,

                    Children = new Drawable[]
                    {
                        new Container
                        {
                            Size = new Vector2(12, 18),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.BottomCentre,
                            Masking = true,
                            CornerRadius = 6,
                            BorderThickness = 6,
                            BorderColour = Color4.Green,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            }
                        },
                        new CircularContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Masking = true,
                            BorderThickness = 6,
                            BorderColour = Color4.Purple,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            }
                        }
                    }
                }
            };
        }

        protected override void Update()
        {
            base.Update();

            Rotation = playerDirectionAngle(player.Position, Position);
        }

        private float playerDirectionAngle(Vector2 playerPos, Vector2 thisPos)
        {
            //Returns a Radian
            float angle = (float) Math.Atan2(playerPos.Y - thisPos.Y, playerPos.X - thisPos.X);
            angle = MathHelper.RadiansToDegrees(angle) + 90;

            return angle;
        }
    }
}
