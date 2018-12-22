using eden.Game.Tests.Visual;
using osu.Framework;
using osu.Framework.Platform;

namespace eden.Game.Tests
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using (GameHost host = Host.GetSuitableHost("Eden Tests"))
            {
                host.Run(new EdenTests());
            }
        }
    }
}
