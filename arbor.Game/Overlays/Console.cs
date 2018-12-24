using System;
using System.Collections.Generic;
using System.Linq;
using arbor.Game.Input;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Bindings;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;

namespace arbor.Game.Overlays
{
    public class Console : OverlayContainer, IKeyBindingHandler<ArborKeyBindings>
    {
        #region static

        public const string CONSOLE_LOG_TYPE = "console";

        private static readonly List<WeakReference> commands = new List<WeakReference>();

        public static void RegisterCommand(ConsoleCommand command)
        {
            commands.Add(new WeakReference(command));
        }

        public static void ExecuteCommand(string command)
        {
            Logger.Log(command, CONSOLE_LOG_TYPE);
            commands.RemoveAll(reference => !reference.IsAlive);
            foreach (var consoleCommand in commands.Select(reference => reference.Target).OfType<ConsoleCommand>().Where(consoleCommand => command.StartsWith(consoleCommand.CommandName)))
            {
                try
                {
                    consoleCommand.CommandAction(command.Substring(consoleCommand.CommandName.Length).Trim());
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"An Exception occurered when trying to execute command \"{consoleCommand.CommandName}\"", CONSOLE_LOG_TYPE);
                }
            }
        }

        #endregion

        #region overlay

        private readonly TextFlowContainer consoleLog;

        private const float height = 400;
        private const float toggle_time = 200;

        private readonly Container content;

        protected override Container<Drawable> Content => content;

        public override bool HandleNonPositionalInput => true;

        protected override bool BlockPositionalInput => false;

        public Console()
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

                    Child = consoleLog = new TextFlowContainer(text => text.TextSize = 20)
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
                ExecuteCommand(sender.Text);
                sender.Text = string.Empty;
            };

            Logger.NewEntry += log;
        }

        private void log(LogEntry logEntry)
        {
            if (logEntry.Level < LogLevel.Important && logEntry.LoggerName != CONSOLE_LOG_TYPE)
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

        public bool OnPressed(ArborKeyBindings action)
        {
            if (action == ArborKeyBindings.Console)
            {
                ToggleVisibility();
                return true;
            }

            return false;
        }

        public bool OnReleased(ArborKeyBindings action) => false;

        #endregion
    }

    public class ConsoleCommand
    {
        public ConsoleCommand(string commandName, Action<string> commandAction)
        {
            CommandAction = commandAction;
            CommandName = commandName;
        }

        public Action<string> CommandAction { get; set; }

        public string CommandName { get; set; }
    }
}
