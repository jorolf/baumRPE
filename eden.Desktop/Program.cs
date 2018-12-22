using System;
using System.Diagnostics;
using System.IO;
using eden.Game;
using osu.Framework;
using osu.Framework.Platform;

namespace eden.Desktop
{
    public static class Program
    {
        [DebuggerNonUserCode]
        public static void Main(string[] args)
        {
            using (GameHost host = Host.GetSuitableHost("Eden"))
            {
                if (args.Length > 1 && args[0] == "--workspace")
                    host.Run(new EdenGame(Environment.ExpandEnvironmentVariables(args[1])));
                else
                    host.Run(new EdenGame(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "eden.Game.Story.dll")));
            }
        }
    }
}
