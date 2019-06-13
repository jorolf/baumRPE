using System.Collections.Generic;
using arbor.Game.Overlays;
using arbor.Game.Overlays.Console;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace arbor.Game.Tests.Visual
{
    public class TestSceneConsole : TestScene
    {
        private readonly ConsoleCommand[] commands = new ConsoleCommand[2];

        public override IReadOnlyList<System.Type> RequiredTypes => new[] { typeof(ConsoleOverlay) };

        [Resolved]
        private ConsoleCommandManager consoleManager { get; set; }

        [Resolved]
        private ConsoleOverlay consoleOverlay { get; set; }

        protected override void LoadComplete()
        {
            Box box;

            Add(box = new Box
            {
                Size = new Vector2(100)
            });

            consoleManager.RegisterCommand(commands[0] = new ConsoleCommand("box size", s =>
            {
                int size;
                if (int.TryParse(s, out size))
                {
                    box.Size = new Vector2(size);
                    Logger.Log("\01Size of box set to " + size, ConsoleCommandManager.CONSOLE_LOG_TYPE);
                }
                else
                {
                    Logger.Log($"\03\"{s}\" is not a valid number", ConsoleCommandManager.CONSOLE_LOG_TYPE);
                }
            }));

            consoleManager.RegisterCommand(commands[1] = new ConsoleCommand("box red", s =>
            {
                bool red;
                if (bool.TryParse(s, out red))
                {
                    box.Colour = red ? Color4.Red : Color4.White;
                    Logger.Log("\01Color set", ConsoleCommandManager.CONSOLE_LOG_TYPE);
                }
                else
                {
                    Logger.Log($"\03\"{s}\" is neither \"true\" nor \"false\"", ConsoleCommandManager.CONSOLE_LOG_TYPE);
                }
            }));

            AddAssert("white box", () => box.Colour == Color4.White);
            AddStep("command: \"box red true\"", () => consoleManager.ExecuteCommand("box red " + bool.TrueString));
            AddAssert("red box", () => box.Colour == Color4.Red);
            AddAssert("small box", () => box.Size == new Vector2(100));
            AddStep("command: \"box size 200\"", () => consoleManager.ExecuteCommand("box size 200"));
            AddAssert("big box", () => box.Size == new Vector2(200));
            AddToggleStep("Show Overlay", b => consoleOverlay.State = b ? Visibility.Visible : Visibility.Hidden);
        }
    }
}
