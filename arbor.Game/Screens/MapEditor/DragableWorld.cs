using System;
using arbor.Game.Worlds;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace arbor.Game.Screens.MapEditor
{
    public class DragableWorld : CompositeDrawable
    {
        private readonly Container transformContainer;
        private readonly Box highlightTile;

        private World world;

        public World World
        {
            get => world;
            set
            {
                if (world == value) return;

                if (world != null)
                    transformContainer.Remove(world);

                world = value;

                if (world != null)
                {
                    transformContainer.Add(world);
                    ResetView();
                }
            }
        }

        public DragableWorld()
        {
            InternalChild = transformContainer = new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Child = highlightTile = new Box
                {
                    Size = new Vector2(Chunk.TILE_SIZE),
                    Alpha = 0,
                    Depth = -1,
                }
            };
        }

        private float zoom = 1;

        protected override bool OnScroll(ScrollEvent scrollEvent)
        {
            float diff = scrollEvent.ScrollDelta.X + scrollEvent.ScrollDelta.Y;

            if (zoom + diff > 3 || zoom + diff < -5) return true;

            zoom += diff;
            transformContainer.Position *= (float)Math.Pow(1.5, diff);
            transformContainer.Scale = new Vector2((float)Math.Pow(1.5, zoom));
            return true;
        }

        private bool dragging;

        protected override bool OnMouseDown(MouseDownEvent mouseDownEvent) => dragging = mouseDownEvent.Button == MouseButton.Middle;

        protected override bool OnMouseMove(MouseMoveEvent mouseMoveEvent)
        {
            if (GetTileAt(mouseMoveEvent.ScreenSpaceMousePosition) is Vector2I tile)
            {
                highlightTile.Position = new Vector2(tile.X * Chunk.TILE_SIZE, tile.Y * Chunk.TILE_SIZE);
                highlightTile.Alpha = 0.1f;
            }
            else
                highlightTile.Alpha = 0;

            if (dragging)
                transformContainer.Position += mouseMoveEvent.Delta;
            return dragging;
        }

        protected override bool OnMouseUp(MouseUpEvent mouseUpEvent) => !(dragging = mouseUpEvent.Button != MouseButton.Middle && dragging);

        protected override void OnHoverLost(HoverLostEvent e) => highlightTile.Alpha = 0;

        public void ResetView()
        {
            transformContainer.Position = Vector2.Zero;
            transformContainer.Scale = Vector2.One;
        }

        public Vector2I? GetTileAt(Vector2 mouseScreenPos)
        {
            var localPos = transformContainer.ToLocalSpace(mouseScreenPos);

            if (world.DrawRectangle.Contains(localPos))
                return new Vector2I((int)(localPos.X / Chunk.TILE_SIZE), (int)(localPos.Y / Chunk.TILE_SIZE));

            return null;
        }
    }
}
