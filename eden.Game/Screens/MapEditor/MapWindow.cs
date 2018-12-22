using System;
using eden.Game.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using OpenTK.Graphics;

namespace eden.Game.Screens.MapEditor
{
    public abstract class MapWindow<T> : EdenWindow
    {
        private readonly Container content;
        private readonly Button submitBtn;
        private const int submit_btn_height = 20;

        protected override Container<Drawable> Content => content;

        public event Action<T> OnSubmit;

        protected string SubmitText
        {
            get => submitBtn.Text;
            set => submitBtn.Text = value;
        }

        protected MapWindow()
        {
            base.Content.AddRange(new Drawable[]
            {
                content = new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Padding = new MarginPadding {Bottom = submit_btn_height + 3}
                },
                submitBtn = new Button
                {
                    CornerRadius = 3,
                    Masking = true,
                    BackgroundColour = Color4.DarkGray,
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.X,
                    Height = submit_btn_height,
                    Action = Submit,
                }
            });
        }

        protected void Submit()
        {
            if (!ValuesValid())
            {
                submitBtn.FlashColour(Color4.Red, 120);
                return;
            }

            OnSubmit?.Invoke(SubmitParam);
            Hide();
        }

        public override void Show()
        {
            ResetValues();
            base.Show();
        }

        protected abstract T SubmitParam { get; }

        protected abstract bool ValuesValid();

        protected abstract void ResetValues();

        protected class NamedContainer : CompositeDrawable
        {
            private readonly Container content;
            private readonly SpriteText nameSprite;

            public override bool HandleNonPositionalInput => true;
            public override bool HandlePositionalInput => true;

            public NamedContainer(string name, Drawable drawable)
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;
                AddRangeInternal(new Drawable[]
                {
                    nameSprite = new SpriteText
                    {
                        Text = name,
                        TextSize = 20,
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                    },
                    content = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Child = drawable
                    }
                });
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                ScheduleAfterChildren(() => content.Padding = new MarginPadding { Left = nameSprite.DrawWidth });
                ScheduleAfterChildren(() => content.RelativeSizeAxes = RelativeSizeAxes);
                ScheduleAfterChildren(() => content.AutoSizeAxes = AutoSizeAxes);
            }
        }
    }
}
