using System.Collections.Generic;
using arbor.Game.Overlays;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace arbor.Game.Tests.Visual
{
    public class TestCaseConsole : TestCase
    {
        private readonly ConsoleCommand[] commands = new ConsoleCommand[2];

        public override IReadOnlyList<System.Type> RequiredTypes => new[] { typeof(Console) };

        public TestCaseConsole()
        {
            Box box;

            Add(box = new Box
            {
                Size = new Vector2(100)
            });

            Console.RegisterCommand(commands[0] = new ConsoleCommand("box size", s =>
            {
                int size;
                if (int.TryParse(s, out size))
                {
                    box.Size = new Vector2(size);
                    Logger.Log("\01Size of box set to " + size, Console.CONSOLE_LOG_TYPE);
                }
                else
                {
                    Logger.Log($"\03\"{s}\" is not a valid number", Console.CONSOLE_LOG_TYPE);
                }
            }));

            Console.RegisterCommand(commands[1] = new ConsoleCommand("box red", s =>
            {
                bool red;
                if (bool.TryParse(s, out red))
                {
                    box.Colour = red ? Color4.Red : Color4.White;
                    Logger.Log("\01Color set", Console.CONSOLE_LOG_TYPE);
                }
                else
                {
                    Logger.Log($"\03\"{s}\" is neither \"true\" nor \"false\"", Console.CONSOLE_LOG_TYPE);
                }
            }));

            AddAssert("white box", () => box.Colour == Color4.White);
            AddStep("command: \"box red true\"", () => Console.ExecuteCommand("box red " + bool.TrueString));
            AddAssert("red box", () => box.Colour == Color4.Red);
            AddAssert("small box", () => box.Size == new Vector2(100));
            AddStep("command: \"box size 200\"", () => Console.ExecuteCommand("box size 200"));
            AddAssert("big box", () => box.Size == new Vector2(200));
        }
    }
}
