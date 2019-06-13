using System;

namespace arbor.Game.Overlays
{
    public class ConsoleCommand
    {
        public ConsoleCommand(string commandName, Action<string> commandAction)
        {
            CommandAction = commandAction;
            CommandName = commandName;
        }

        public Action<string> CommandAction { get; }

        public string CommandName { get; }

        protected bool Equals(ConsoleCommand other) => string.Equals(CommandName, other.CommandName);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals((ConsoleCommand)obj);
        }

        public override int GetHashCode() => (CommandName != null ? CommandName.GetHashCode() : 0);
    }
}
