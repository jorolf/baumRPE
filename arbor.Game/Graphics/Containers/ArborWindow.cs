using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace arbor.Game.Graphics.Containers
{
    public class ArborWindow : OverlayContainer
    {
        private readonly Container content;
        private readonly SpriteText windowTitle;
        private readonly Container hideButton;

        protected override Container<Drawable> Content => content;

        public string Title
        {
            get => windowTitle.Text;
            set => windowTitle.Text = value;
        }

        public bool Closeable
        {
            get => hideButton.Alpha == 1;
            set => hideButton.Alpha = value ? 1 : 0;
        }

        public ArborWindow()
        {
            Masking = true;
            CornerRadius = 5;
            AutoSizeAxes = Axes.Both;

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    Colour = Color4.Black,
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.25f
                },
                content = new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Padding = new MarginPadding(5) {Top = 20}
                },
                new DragBar
                {
                    Colour = Color4.Black,
                    Alpha = 0.2f,
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(1, 20),
                    DragAction = delta => Position += delta
                },
                hideButton = new ClickableContainer
                {
                    Size = new Vector2(20),
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Action = Hide,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Red
                    },
                },
                windowTitle = new SpriteText
                {
                    TextSize = 20,
                    Margin = new MarginPadding {Left = 3}
                }
            });
        }

        protected override void PopIn() => Alpha = 1;

        protected override void PopOut() => Alpha = 0;

        protected override bool OnMouseDown(MouseDownEvent e) => true;

        private class DragBar : Box
        {
            public Action<Vector2> DragAction;

            protected override bool OnDragStart(DragStartEvent dragStartEvent) => true;

            protected override bool OnDrag(DragEvent dragEvent)
            {
                DragAction(dragEvent.Delta);
                return true;
            }

            protected override bool OnDragEnd(DragEndEvent dragEndEvent) => true;
        }
    }
}
