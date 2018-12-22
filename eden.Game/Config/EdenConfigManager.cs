using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace eden.Game.Config
{
    public class EdenConfigManager : IniConfigManager<EdenSetting>
    {
        protected override string Filename => @"eden.ini";

        public EdenConfigManager(Storage storage) : base(storage)
        {
        }

        protected override void InitialiseDefaults()
        {
        }
    }

    public enum EdenSetting
    {
    }
}
