using System;
using System.Collections.Generic;
using eden.Game.IO;
using eden.Game.Screens.MapEditor;
using eden.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;

namespace eden.Game.Tests.Visual
{
    public class TestCaseTileAtlasWindow : TestCase
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] {typeof(TileAtlasWindow), typeof(DrawableTile)};

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            DependencyContainer depedencyContainer = new DependencyContainer(parent);

            depedencyContainer.Cache(new Story
            {
                ResourceFiles = new List<string>
                {
                    "World/Tiles/dirtTile.png",
                    "World/Tiles/grassTile.png",
                    "World/Tiles/nullTile.png"
                }
            });

            return base.CreateChildDependencies(depedencyContainer);
        }

        [BackgroundDependencyLoader]
        private void load(JsonStore jsonStore, TileStore textureStore)
        {
            TileAtlas tileAtlas = new TileAtlas(textureStore, jsonStore, "Json/tiles.json");
            Add(new TileAtlasWindow(tileAtlas)
            {
                State = Visibility.Visible
            });
        }
    }
}
