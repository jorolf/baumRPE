using System;
using System.Collections.Generic;
using arbor.Game.IO;
using arbor.Game.Screens.MapEditor;
using arbor.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;

namespace arbor.Game.Tests.Visual
{
    public class TestSceneTileAtlasWindow : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] { typeof(TileAtlasWindow), typeof(DrawableTile) };

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            DependencyContainer deparborcyContainer = new DependencyContainer(parent);

            deparborcyContainer.Cache(new Story
            {
                ResourceFiles = new List<string>
                {
                    "World/Tiles/dirtTile.png",
                    "World/Tiles/grassTile.png",
                    "World/Tiles/nullTile.png"
                }
            });

            return base.CreateChildDependencies(deparborcyContainer);
        }

        [BackgroundDependencyLoader]
        private void load(JsonStore jsonStore, TileStore textureStore)
        {
            TileAtlas tileAtlas = new TileAtlas(textureStore, jsonStore, "Json/tiles.json");
            Add(new TileAtlasWindow
            {
                State = Visibility.Visible,
                TileAtlas = tileAtlas,
            });
        }
    }
}
