using osu.Framework.Platform;

namespace eden.Game.IO
{
    public class StoryStorage : DesktopStorage
    {
        private readonly string basePath;

        private static string staticBasePath;

        public StoryStorage(string basePath, GameHost host) : base(setBasePath(basePath), host)
        {
            this.basePath = basePath;
        }

        protected override string LocateBasePath() => basePath ?? staticBasePath;

        private static string setBasePath(string basePath)
        {
            staticBasePath = basePath;
            return string.Empty;
        }
    }
}
