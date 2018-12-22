using System;
using System.Collections.Generic;
using eden.Game.Graphics.Containers;
using eden.Game.Screens.MapEditor;
using eden.Game.Worlds;
using osu.Framework.Graphics;
using osu.Framework.Testing;

namespace eden.Game.Tests.Visual
{
    public class TestCaseDragableWorld : TestCase
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] {typeof(DragableWorld), typeof(EdenDragContainer)};

        public TestCaseDragableWorld()
        {
            World world = new World();
            world.ResizeWorld(1,1);

            Child = new DragableWorld
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                World = world,
            };
        }
    }
}
