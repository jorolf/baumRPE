using System;
using System.Collections.Generic;
using System.Linq;
using eden.Game.IO;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace eden.Game.Worlds
{
    [JsonObject(MemberSerialization.OptIn)]
    public class World : Container
    {
        private JsonStore jsonStore;
        private TextureStore textureStore;

        private string atlasPath;

        public string WorldName;

        [JsonProperty("sprites")]
        public string AtlasPath
        {
            get => Atlas?.Filename ?? atlasPath;
            set
            {
                atlasPath = value;
                if (jsonStore != null) createAtlas(value);
            }
        }

        public TileAtlas Atlas { get; private set; }

        [JsonProperty("spawns")]
        public List<Vector2> Spawns = new List<Vector2>();

        [BackgroundDependencyLoader]
        private void load(JsonStore store, TileStore tiles)
        {
            AutoSizeAxes = Axes.Both;
            jsonStore = store;
            textureStore = tiles;
            for (var x = 0; x < chunks.GetLength(0); x++)
            for (var y = 0; y < chunks.GetLength(1); y++)
            {
                var chunk = chunks[x, y];
                chunk.Position = new Vector2(x, y) * Chunk.CHUNK_SIZE;
                Add(chunk);
            }

            if (atlasPath != null)
                createAtlas(atlasPath);
        }

        private void createAtlas(string filename)
        {
            Atlas = new TileAtlas(textureStore, jsonStore, filename);
            foreach (var chunk in chunks)
                chunk.Atlas = Atlas; //also update the sprites
        }

        [JsonProperty]
        private Chunk[,] chunks = new Chunk[0, 0];

        /// <summary>
        /// Resize this world to the specified size.
        /// Size is measured in chunks
        /// </summary>
        /// <param name="width">Chunks on the x axis</param>
        /// <param name="height">Chunks on the y axis</param>
        public void ResizeWorld(int width, int height)
        {
            if (!IsAlive) //The world isn't loaded and the chunks aren't setup yet
            {
                Schedule(() => ResizeWorld(width, height));
                return;
            }

            var oldChunks = chunks;
            chunks = new Chunk[width, height];

            for (var x = 0; x < chunks.GetLength(0); x++)
            {
                for (var y = 0; y < chunks.GetLength(1); y++)
                {
                    if (oldChunks.GetLength(0) > x && oldChunks.GetLength(1) > y)
                        chunks[x, y] = oldChunks[x, y];
                    else
                    {
                        var chunk = new Chunk
                        {
                            Position = new Vector2(x, y) * Chunk.CHUNK_SIZE,
                            Atlas = Atlas
                        };
                        Add(chunk);
                        chunks[x, y] = chunk;
                    }
                }
            }

            RemoveRange(oldChunks.Cast<Chunk>().Except(chunks.Cast<Chunk>()));
        }

        public Chunk GetChunk(int x, int y) => chunks[x, y];

        /// <summary>
        /// Size of this world in chunks
        /// </summary>
        public Vector2 ChunkSize => new Vector2(chunks.GetLength(0), chunks.GetLength(1));

        public void Save()
        {
            if (string.IsNullOrEmpty(WorldName))
                throw new InvalidOperationException(nameof(WorldName) + " is null or empty and can therefore not be saved");

            jsonStore.Serialize(WorldName, this);
        }
    }
}
