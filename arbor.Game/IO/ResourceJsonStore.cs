using System;
using System.IO;
using osu.Framework.IO.Stores;

namespace arbor.Game.IO
{
    public class ResourceJsonStore : JsonStore
    {
        private readonly IResourceStore<byte[]> store;

        public ResourceJsonStore(IResourceStore<byte[]> store)
        {
            this.store = store;
        }

        protected override Stream GetFileStream(string filename, FileAccess fileAccess)
        {
            if ((fileAccess & FileAccess.Write) != 0)
                throw new ArgumentException($"{nameof(ResourceJsonStore)} cannot write to files!");

            var buffer = store.Get(filename);

            return buffer != null ? new MemoryStream(buffer) : null;
        }
    }
}
