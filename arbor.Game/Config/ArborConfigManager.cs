using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace arbor.Game.Config
{
    public class ArborConfigManager : IniConfigManager<ArborSetting>
    {
        protected override string Filename => @"arbor.ini";

        public ArborConfigManager(Storage storage)
            : base(storage)
        {
        }

        protected override void InitialiseDefaults()
        {
        }
    }

    public enum ArborSetting
    {
    }
}
