using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using osuTK;

namespace arbor.Game.IO
{
    public abstract class JsonStore
    {
        private readonly JsonSerializerSettings settings;

        protected JsonStore()
        {
            settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new VectorConverter()
                }
            };
        }

        /// <summary>
        /// Load the JSON string of the specified file and deserializes it
        /// </summary>
        /// <typeparam name="T">The object that should be populated</typeparam>
        /// <param name="filename">The file to load the JSON string from</param>
        /// <returns></returns>
        public T Deserialize<T>(string filename)
        {
            var stream = GetFileStream(filename, FileAccess.Read);
            if (stream == null)
                throw new FileNotFoundException($"The file {filename} does not exist!", filename);

            using (var reader = new StreamReader(stream))
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), settings);
        }

        /// <summary>
        /// Serializes an object and saves it to the specified file
        /// </summary>
        /// <param name="filename">Location of the saved file</param>
        /// <param name="obj">The serialization object</param>
        public void Serialize(string filename, object obj)
        {
            using (var writer = new StreamWriter(GetFileStream(filename, FileAccess.Write)))
                writer.Write(JsonConvert.SerializeObject(obj, settings));
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

        public class VectorConverter : JsonConverter<Vector2>
        {
            public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, new[] { value.X, value.Y });
            }

            public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var floats = serializer.Deserialize<float[]>(reader);
                switch (floats.Length)
                {
                    case 0:
                        return Vector2.Zero;
                    case 1:
                        return new Vector2(floats[0]);
                    default:
                        return new Vector2(floats[0], floats[1]);
                }
            }
        }
    }
}
