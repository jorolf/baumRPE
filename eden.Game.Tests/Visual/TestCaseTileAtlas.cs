using System;
using System.Collections.Generic;
using eden.Game.IO;
using eden.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;
using OpenTK;

namespace eden.Game.Tests.Visual
{
    public class TestCaseTileAtlas : TestCase
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] {typeof(TileAtlas), typeof(DrawableTile)};

        private const string world = @"Json/tiles.json";

        [BackgroundDependencyLoader]
        private void load(TileStore textures, JsonStore jsonStore)
        {
            TileAtlas atlas = new TileAtlas(textures, jsonStore, world);

            Add(new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Children = new[]
                {
                    new DrawableTile(atlas[0])
                    {
                        Size = new Vector2(Chunk.TILE_SIZE)
                    },
                    new DrawableTile(atlas[1])
                    {
                        Size = new Vector2(Chunk.TILE_SIZE)
                    }
                }
            });
        }
    }
}
