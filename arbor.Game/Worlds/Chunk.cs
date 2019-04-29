using System.Linq;
using arbor.Game.Gameplay;
using Newtonsoft.Json;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osuTK;

namespace arbor.Game.Worlds
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Chunk : Container
    {
        public const int TILES_PER_CHUNK = 16;
        public const int TILE_SIZE = 64;
        public const int CHUNK_SIZE = TILES_PER_CHUNK * TILE_SIZE;

        [JsonProperty("tiles")]
        public int?[,] Tiles
        {
            get => tileContainer.Tiles;
            set => tileContainer.Tiles = value;
        }

        public TileAtlas Atlas
        {
            get => tileContainer.Atlas;
            set => tileContainer.Atlas = value;
        }

        private readonly DrawableTiles tileContainer;
        private readonly Container<GameObject> objectContainer;

        public Chunk()
        {
            Size = new Vector2(CHUNK_SIZE);
            AddRange(new Drawable[]
            {
                tileContainer = new DrawableTiles
                {
                    RelativeSizeAxes = Axes.Both
                },
                objectContainer = new Container<GameObject>
                {
                    RelativeSizeAxes = Axes.Both
                }
            });
        }

        [JsonProperty("objects", TypeNameHandling = TypeNameHandling.Auto)]
        private GameObject[] objects
        {
            get => objectContainer.Children.ToArray();
            set
            {
                objectContainer.Clear();
                objectContainer.AddRange(value);
            }
        }

        public void AddObject(GameObject obj) => objectContainer.Add(obj);

        public void RemoveObject(GameObject obj) => objectContainer.Remove(obj);

        public GameObject GetObjectAt(Vector2I pos) => objectContainer.Children.FirstOrDefault(obj => obj.WorldPosition.Equals(pos));
    }
}
