using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using eden.Game.IO;
using Newtonsoft.Json;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace eden.Game.Worlds
{
    public class TileAtlas : Collection<Tile>
    {
        public readonly TextureStore TextureStore;
        private readonly JsonStore jsonStore;
        public readonly string Filename;

        public TileAtlas(TextureStore textureStore, JsonStore jsonStore, string filename)
        {
            TextureStore = textureStore;
            this.jsonStore = jsonStore;
            Filename = filename;

            try
            {
                jsonStore.Populate(filename, this, true);
            }
            catch (FileNotFoundException)
            {
            }
        }

        protected override void InsertItem(int index, Tile item)
        {
            base.InsertItem(index, item);
            item.LoadTextures(TextureStore);
        }

        protected override void SetItem(int index, Tile item)
        {
            base.SetItem(index, item);
            item.LoadTextures(TextureStore);
        }

        public void Save() => jsonStore.Serialize(Filename, this, true);
    }

    public abstract class Tile
    {
        [JsonProperty("solid")]
        public bool Solid;

        public abstract void LoadTextures(TextureStore textures);

        public abstract Texture GetTexture(double time);
    }

    public class AnimatedTile : Tile
    {
        [JsonProperty("frames")]
        public List<string> Frames = new List<string>();

        [JsonProperty("speed")]
        public float Speed;

        private List<Texture> frameTextures;


        public override void LoadTextures(TextureStore textures)
        {
            frameTextures = Frames.Select(textures.Get).ToList();
        }

        public override Texture GetTexture(double time)
        {
            return frameTextures[(int) (time / Speed) % frameTextures.Count];
        }
    }

    public class StaticTile : Tile
    {
        [JsonProperty]
        public string File;

        private Texture texture;

        public override void LoadTextures(TextureStore textures)
        {
            texture = textures.Get(File);
        }

        public override Texture GetTexture(double time)
        {
            return texture;
        }
    }

    public class BlankTile : Tile
    {
        public override void LoadTextures(TextureStore textures)
        {
        }

        public override Texture GetTexture(double time) => Texture.WhitePixel;
    }
}
