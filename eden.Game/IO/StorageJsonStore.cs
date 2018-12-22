using System.IO;
using osu.Framework.Platform;

namespace eden.Game.IO
{
    public class StorageJsonStore : JsonStore
    {
        private readonly Storage storage;

        public StorageJsonStore(Storage storage)
        {
            this.storage = storage;
        }

        protected override Stream GetFileStream(string filename, FileAccess fileAccess) => storage.GetStream(filename, fileAccess, (fileAccess & FileAccess.Write) == 0 ? FileMode.Open : FileMode.Create);
    }
}
