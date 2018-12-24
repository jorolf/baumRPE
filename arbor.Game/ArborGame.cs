using System;
using System.IO;
using arbor.Game.IO;
using arbor.Game.Screens;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;

namespace arbor.Game
{
    public class ArborGame : ArborBaseGame
    {
        private readonly string storyPath;
        public readonly IResourceStore<byte[]> StoryResources;
        public Storage StoryStorage;

        private DependencyContainer dependencies;
        private readonly Story story;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public ArborGame(string storyPath)
        {
            this.storyPath = storyPath;
            if (File.Exists(storyPath) && Path.GetExtension(storyPath) == ".dll")
                StoryResources = new DllResourceStore(storyPath);
            else
            {
                Directory.CreateDirectory(storyPath);
                StoryStorage = new StoryStorage(storyPath, null);
                StoryResources = new StorageBackedResourceStore(StoryStorage);

                if (!StoryStorage.Exists(Story.FILENAME))
                {
                    story = new Story
                    {
                        Name = "New story"
                    };
                    using (var writer = new StreamWriter(StoryStorage.GetStream(Story.FILENAME, FileAccess.Write)))
                        writer.Write(JsonConvert.SerializeObject(story));

                    return;
                }
            }

            var storyFileArray = StoryResources.Get(Story.FILENAME);

            if (storyFileArray == null) throw new ArgumentException($"{storyPath} isn't valid as it doesn't contain a story file!");

            story = JsonConvert.DeserializeObject<Story>(System.Text.Encoding.UTF8.GetString(storyFileArray));
        }

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            if(StoryResources is StorageBackedResourceStore) StoryStorage = new StoryStorage(storyPath, host);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            dependencies.Cache(this);
            dependencies.Cache(story);
            dependencies.CacheAs(StoryStorage == null
                ? new ResourceJsonStore(Resources)
                : (JsonStore) new StorageJsonStore(StoryStorage));

            dependencies.Cache(new TileStore(new TextureLoaderStore(StoryResources)));
            Resources.AddStore(StoryResources);

            var homeScreen = new HomeScreen();

            Add(homeScreen);

            homeScreen.Exited += _ => Scheduler.AddDelayed(Exit, 500);
        }
    }
}
