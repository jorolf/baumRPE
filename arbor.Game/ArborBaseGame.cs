using System;
using arbor.Game.Config;
using arbor.Game.Input;
using arbor.Game.IO;
using arbor.Game.Overlays;
using arbor.Game.Overlays.Console;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;

namespace arbor.Game
{
    [Cached]
    public class ArborBaseGame : osu.Framework.Game
    {
        private ArborConfigManager localConfig;

        private GlobalKeyBindingInputManager inputManager;
        private Container content;

        protected override Container<Drawable> Content => content;

        private DependencyContainer dependencies;

        [Cached]
        private ConsoleCommandManager consoleManager = new ConsoleCommandManager();

        [Cached]
        private ConsoleOverlay consoleOverlay = new ConsoleOverlay();

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore("arbor.Game.Resources.dll"));

            //dependencies.Cache(this);
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
                Child = new BasicContextMenuContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = content = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            consoleOverlay.With(d =>
                            {
                                d.Depth = -1;
                                d.RelativeSizeAxes = Axes.Both;
                            })
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

        public bool OnPressed(ArborKeyBindings action)
        {
            switch (action)
            {
                case ArborKeyBindings.Console:
                    consoleOverlay.ToggleVisibility();
                    return true;
            }

            return false;
        }

        public bool OnReleased(ArborKeyBindings action) => false;
    }
}
