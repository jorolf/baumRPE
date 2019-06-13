using arbor.Game.Tests.Visual;
using osu.Framework;
using osu.Framework.Platform;

namespace arbor.Game.Tests
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using (GameHost host = Host.GetSuitableHost("Arbor"))
            {
                host.Run(new ArborTests());
            }
        }
    }
}
