using arbor.Game.Config;
using arbor.Game.Input;
using arbor.Game.IO;
using arbor.Game.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;

namespace arbor.Game
{
    public class ArborBaseGame : osu.Framework.Game
    {
        private ArborConfigManager localConfig;

        private GlobalKeyBindingInputManager inputManager;
        private Container content;

        protected override Container<Drawable> Content => content;

        private DependencyContainer dependencies;


        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore("arbor.Game.Resources.dll"));

            dependencies.Cache(this);
            dependencies.Cache(localConfig);
            dependencies.CacheAs<JsonStore>(new ResourceJsonStore(Resources));
            dependencies.Cache(new TileStore(new TextureLoaderStore(new NamespacedResourceStore<byte[]>(Resources, "Textures"))));
        }

        public override void SetHost(GameHost host)
        {
            if (localConfig == null)
                localConfig = new ArborConfigManager(host.Storage);

            base.SetHost(host);

            Window.Title = @"ArborEngine";
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            base.Content.Add(inputManager = new GlobalKeyBindingInputManager(Host.Storage)
            {
                Child = content = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Console
                        {
                            Depth = -1,
                            RelativeSizeAxes = Axes.Both
                        }
                    }
                }
            });
        }

        protected override void Dispose(bool isDisposing)
        {
            localConfig?.Save();
            inputManager.SaveKeyBindings();

            base.Dispose(isDisposing);
        }
    }
}
