using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace eden.Game.Graphics.UserInterface
{
    public class MultiOption<T> : Container where T : IConvertible
    {
        public T Selected { get; set; }

        private Button centreButton;

        public MultiOption()
        {
            Masking = false;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new Button
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = 40,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreRight,
                    Text = "",
                    Scale = new Vector2(0.6f),
                    Action = previousOption
                },
                centreButton = new Button
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = Enum.GetName(typeof(T), Selected),
                    Action = nextOption
                },
                new Button
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = 40,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Text = "",
                    Scale = new Vector2(-0.6f, 0.6f),
                    Action = nextOption
                }
            };
        }

        private void nextOption()
        {
            centreButton.Text = Enum.GetName(typeof(T), Selected);
        }

        private void previousOption()
        {
        }
    }
}
