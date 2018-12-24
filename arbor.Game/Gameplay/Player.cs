using System;
using arbor.Game.Gameplay.Characters;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace arbor.Game.Gameplay
{
    /// <summary>
    /// This will be the player class (the person you control).
    /// </summary>
    public abstract class Player : Witch, IRequireHighFrequencyMousePosition
    {
        private Vector2 velocity;
        private bool shiftPressed;
        public RectangleF PlayerBounds;

        public const float PLAYER_SPEED = 0.15f;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        protected Player()
        {
            AlwaysPresent = true;
            Origin = Anchor.Centre; //Todo: this shouldn't done

            Size = new Vector2(24); //Todo: this should be done in World

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
                    BorderColour = Color4.Blue,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    }
                },
                new Circle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    BorderThickness = 6,
                    BorderColour = Color4.Red,
                }
            };
        }

        protected override void Update()
        {
            base.Update();
            playerMovement();
        }

        private void playerMovement()
        {
            //Handles Player Speed

            Vector2 playerPosition = Position;

            playerPosition += velocity * (float) Clock.ElapsedFrameTime * (shiftPressed ? 2 : 1);

            if (PlayerBounds != default(RectangleF))
            {
                playerPosition = Vector2.ComponentMin(playerPosition, PlayerBounds.BottomRight);
                playerPosition = Vector2.ComponentMax(playerPosition, PlayerBounds.TopLeft);
            }

            Position = playerPosition;
        }

        private float mousePosAngle(Vector2 mousePos, Vector2 playerPos)
        {
            //Returns a Radian
            float angle = (float) Math.Atan2(mousePos.Y - playerPos.Y, mousePos.X - playerPos.X);
            angle = MathHelper.RadiansToDegrees(angle) + 90;
            return angle;
        }

        protected override bool OnKeyDown(KeyDownEvent keyDownEvent)
        {
            if (keyDownEvent.Repeat)
                return base.OnKeyDown(keyDownEvent);
            shiftPressed = keyDownEvent.ShiftPressed;
            switch (keyDownEvent.Key)
            {
                case Key.W:
                    velocity.Y = -PLAYER_SPEED;
                    break;
                case Key.S:
                    velocity.Y = PLAYER_SPEED;
                    break;
                case Key.A:
                    velocity.X = -PLAYER_SPEED;
                    break;
                case Key.D:
                    velocity.X = PLAYER_SPEED;
                    break;
            }

            return base.OnKeyDown(keyDownEvent);
        }

        protected override bool OnKeyUp(KeyUpEvent keyUpEvent)
        {
            shiftPressed = keyUpEvent.ShiftPressed;
            switch (keyUpEvent.Key)
            {
                case Key.W:
                    if (!keyUpEvent.IsPressed(Key.S))
                        velocity.Y = 0;
                    break;
                case Key.S:
                    if (!keyUpEvent.IsPressed(Key.W))
                        velocity.Y = 0;
                    break;
                case Key.A:
                    if (!keyUpEvent.IsPressed(Key.D))
                        velocity.X = 0;
                    break;
                case Key.D:
                    if (!keyUpEvent.IsPressed(Key.A))
                        velocity.X = 0;
                    break;
            }

            return base.OnKeyUp(keyUpEvent);
        }

        protected override bool OnMouseMove(MouseMoveEvent mouseMoveEvent)
        {
            Rotation = mousePosAngle(mouseMoveEvent.MousePosition, Position);
            return base.OnMouseMove(mouseMoveEvent);
        }

        protected override bool OnMouseDown(MouseDownEvent mouseDownEvent)
        {/*
            if (args.Button == MouseButton.Right)
                World.Add(new Ball(World, new Fire(), new Velocity { SpellVelocity = (ToSpaceOfOtherDrawable(state.Mouse.Position, this) - Position).Normalized() / 4 }, new Duration { SpellDuration = 1000 })
                {
                    Shape = Shape.Circle,
                    Team = 1,
                    Damage = 10,
                    WorldStartPosition = Position / Chunk.TILE_SIZE,
                    Size = new Vector2(10)
                });
            else if (args.Button == MouseButton.Left)
                Weapon.Attack();
                */
            return base.OnMouseDown(mouseDownEvent);
        }
    }
}
