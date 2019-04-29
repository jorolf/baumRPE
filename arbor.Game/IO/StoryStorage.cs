using osu.Framework.Platform;

namespace arbor.Game.IO
{
    public class StoryStorage : NativeStorage
    {
        public StoryStorage(string basePath, GameHost host)
            : base(string.Empty, host)
        {
            BasePath = basePath;
        }
    }
}
