using arbor.Game.Overlays.Console;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;

namespace arbor.Game.Overlays
{
    public class ConsoleOverlay : OverlayContainer
    {
        private readonly TextFlowContainer consoleLog;

        private const float height = 400;
        private const float toggle_time = 200;

        private readonly Container content;

        protected override Container<Drawable> Content => content;

        protected override bool BlockPositionalInput => false;

        [Resolved]
        private ConsoleCommandManager consoleManager { get; set; }

        public ConsoleOverlay()
        {
            TextBox input;

            AddInternal(content = new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = height,
                Depth = float.MinValue,
                Position = new Vector2(0, -height),
            });

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f
                },
                new ScrollContainer
                {
                    RelativeSizeAxes = Axes.X,
                    Height = height - 20,

                    Child = consoleLog = new TextFlowContainer(text => text.Font = text.Font.With(size: 20))
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y
                    }
                },
                new Box
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.X,
                    Height = 20,
                    Colour = Color4.Black,
                    Alpha = 0.5f
                },
                input = new TextBox
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.X,
                    Height = 20,
                    ReleaseFocusOnCommit = false
                }
            };

            input.OnCommit += (sender, text) =>
            {
                consoleManager.ExecuteCommand(sender.Text);
                sender.Text = string.Empty;
            };

            Logger.NewEntry += log;
        }

        private void log(LogEntry logEntry)
        {
            if (logEntry.Level < LogLevel.Important && logEntry.LoggerName != ConsoleCommandManager.CONSOLE_LOG_TYPE)
                return;

            Color4 color;
            switch (logEntry.Message.Substring(0, 2))
            {
                default:
                    color = Color4.White;
                    break;
                case "\01":
                    color = Color4.Green;
                    break;
                case "\02":
                    color = Color4.Yellow;
                    break;
                case "\03":
                    color = Color4.Red;
                    break;
            }

            consoleLog.AddText(logEntry.Message.StartsWith("\0") ? logEntry.Message.Substring(2) : logEntry.Message, spriteText => spriteText.Colour = color);
            consoleLog.NewLine();
        }

        protected override void PopIn()
        {
            content.FadeIn(toggle_time)
                   .MoveTo(Vector2.Zero, toggle_time);
        }

        protected override void PopOut()
        {
            content.FadeOut(toggle_time)
                   .MoveTo(new Vector2(0, -height), toggle_time);
        }
    }
}
