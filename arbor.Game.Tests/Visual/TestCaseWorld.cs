using System;
using System.Collections.Generic;
using arbor.Game.IO;
using arbor.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Testing;

namespace arbor.Game.Tests.Visual
{
    public class TestCaseWorld : TestCase
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] {typeof(World), typeof(TileAtlas), typeof(Chunk)};

        private World world;

        [BackgroundDependencyLoader]
        private void load(JsonStore jsonStore)
        {
            Add(world = jsonStore.Deserialize<World>("Json/world.json"));
            //world.Spin(1000, RotationDirection.Clockwise).Loop();
            //world.Anchor = Anchor.Centre;
            //world.Origin = Anchor.Centre;
            world.OnLoadComplete += _ => Schedule(worldLoadComplete);
        }

        private void worldLoadComplete()
        {
            AddStep("resize world", () => world.ResizeWorld(5, 5));
            AddStep("populate tile", () => world.GetChunk(1, 0).Tiles[0, 0] = 1);
            //AddStep("update chunk", () => world.GetChunk(1, 0).CreateTiles(world.Atlas));
        }
    }
}
