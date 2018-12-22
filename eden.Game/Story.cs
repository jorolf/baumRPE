using System.Collections.Generic;
using Newtonsoft.Json;

namespace eden.Game
{
    public class Story
    {
        public const string FILENAME = "story.json";

        [JsonProperty("spawnWorld")]
        public string SpawnWorldFile { get; set; }

        [JsonProperty("spawnID")]
        public int SpawnID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("worldFiles")]
        public List<string> WorldFiles = new List<string>();

        [JsonProperty("atlasFiles")]
        public List<string> AtlasFiles = new List<string>();

        [JsonProperty("resourceFiles")]
        public List<string> ResourceFiles = new List<string>();
    }
}
