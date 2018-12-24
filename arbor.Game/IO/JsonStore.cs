using System.IO;
using Newtonsoft.Json;

namespace arbor.Game.IO
{
    public abstract class JsonStore
    {
        private readonly JsonSerializerSettings settings;

        protected JsonStore()
        {
            settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};
        }

        /// <summary>
        /// Load the JSON string of the specified file and deserializes it
        /// </summary>
        /// <typeparam name="T">The object that should be populated</typeparam>
        /// <param name="filename">The file to load the JSON string from</param>
        /// <param name="useType">Use the "$type" member for the deserialization</param>
        /// <returns></returns>
        public T Deserialize<T>(string filename, bool useType = false)
        {
            var stream = GetFileStream(filename, FileAccess.Read);
            if (stream == null)
                throw new FileNotFoundException($"The file {filename} does not exist!", filename);
            using (var reader = new StreamReader(stream))
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), useType ? settings : null);
        }

        /// <summary>
        /// Serializes an object and saves it to the specified file
        /// </summary>
        /// <param name="filename">Location of the saved file</param>
        /// <param name="obj">The serialization object</param>
        /// <param name="saveType">Save a "$type" member for deserialization</param>
        public void Serialize(string filename, object obj, bool saveType = false)
        {
            using (var writer = new StreamWriter(GetFileStream(filename, FileAccess.Write)))
                writer.Write(JsonConvert.SerializeObject(obj, saveType ? settings : null));
        }

        /// <summary>
        /// Load the JSON string of the specified file and populate <paramref name="obj"/>
        /// </summary>
        /// <param name="filename">The file to load the JSON string from</param>
        /// <param name="obj">The object that should be populated</param>
        /// <param name="useType">Use the "$type" member for the population</param>
        public void Populate(string filename, object obj, bool useType = false)
        {
            var stream = GetFileStream(filename, FileAccess.Read);
            if (stream == null)
                throw new FileNotFoundException($"The file {filename} does not exist!", filename);
            using (var reader = new StreamReader(stream))
                JsonConvert.PopulateObject(reader.ReadToEnd(), obj, useType ? settings : null);
        }

        protected abstract Stream GetFileStream(string filename, FileAccess fileAccess);
    }
}
