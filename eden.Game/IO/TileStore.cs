using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using OpenTK.Graphics.ES30;

namespace eden.Game.IO
{
    public class TileStore : TextureStore
    {
        public TileStore(IResourceStore<TextureUpload> store = null, bool useAtlas = true, All filteringMode = All.Nearest)
            : base(store, useAtlas, filteringMode)
        {
        }
    }
}
