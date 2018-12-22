using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Platform;
using OpenTK;
using OpenTK.Graphics;

namespace eden.Game.Overlays
{
    public class SettingsOverlay : FocusedOverlayContainer
    {
        internal const float CONTENT_MARGINS = 10;

        public const float TRANSITION_LENGTH = 600;

        protected const float WIDTH = 400;

        public SettingsOverlay()
        {
            Anchor = Anchor.CentreRight;
            Origin = Anchor.CentreRight;
            RelativeSizeAxes = Axes.Y;
            Width = WIDTH;

            Children = new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    RelativeSizeAxes = Axes.Y,
                    Width = WIDTH,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0.2f,
                            Colour = Color4.DarkGray
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config, Storage storage)
        {
            Dropdown<FrameSync> frameSyncDropdown;

            AddRange(new Drawable[]
            {
                new Button
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "Open Eden Folder",
                    Size = new Vector2(100, 40),
                    Action = storage.OpenInNativeExplorer,
                    BackgroundColour = Color4.Gray,
                },
                frameSyncDropdown = new BasicDropdown<FrameSync>
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.BottomCentre,
                    Position = new Vector2(0, -100),
                    Width = 80,
                    Items = Enum.GetValues(typeof(FrameSync)).Cast<FrameSync>().ToDictionary(fs => fs.ToString()),
                }
            });

            config.BindWith(FrameworkSetting.FrameSync, frameSyncDropdown.Current);
        }

        protected override void PopIn()
        {
            base.PopIn();

            this.MoveToX(0, TRANSITION_LENGTH, Easing.OutQuint);

            this.FadeTo(1, TRANSITION_LENGTH, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            base.PopOut();

            this.MoveToX(WIDTH, TRANSITION_LENGTH, Easing.OutQuint);

            this.FadeTo(0, TRANSITION_LENGTH, Easing.OutQuint);
        }

        public override bool AcceptsFocus => true;
    }
}
