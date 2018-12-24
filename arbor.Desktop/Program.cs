using System;
using System.Diagnostics;
using System.IO;
using arbor.Game;
using osu.Framework;
using osu.Framework.Platform;

namespace arbor.Desktop
{
    public static class Program
    {
        [DebuggerNonUserCode]
        public static void Main(string[] args)
        {
            using (GameHost host = Host.GetSuitableHost("Arbor"))
            {
                if (args.Length > 1 && args[0] == "--workspace")
                    host.Run(new ArborGame(Environment.ExpandEnvironmentVariables(args[1])));
                else
                    host.Run(new ArborGame(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "arbor.Game.Story.dll")));
            }
        }
    }
}
