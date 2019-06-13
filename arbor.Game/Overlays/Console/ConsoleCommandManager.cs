using System;
using System.Linq;
using osu.Framework.Lists;
using osu.Framework.Logging;

namespace arbor.Game.Overlays.Console
{
    public class ConsoleCommandManager
    {
        public const string CONSOLE_LOG_TYPE = "console";
        private readonly WeakList<ConsoleCommand> commands = new WeakList<ConsoleCommand>();

        public void RegisterCommand(ConsoleCommand command)
        {
            if (!commands.Contains(command))
                commands.Add(command);
        }

        public void ExecuteCommand(string command)
        {
            Logger.Log(command, CONSOLE_LOG_TYPE);
            foreach (var consoleCommand in commands.Where(consoleCommand => command.StartsWith(consoleCommand.CommandName)))
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
    }
}
