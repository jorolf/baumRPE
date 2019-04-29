using System;
using System.Collections.Generic;
using arbor.Game.Graphics.Containers;
using arbor.Game.Screens.MapEditor;
using arbor.Game.Worlds;
using osu.Framework.Graphics;
using osu.Framework.Testing;

namespace arbor.Game.Tests.Visual
{
    public class TestCaseDraggableWorld : TestCase
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] { typeof(DraggableWorld), typeof(ArborDragContainer) };

        public TestCaseDraggableWorld()
        {
            World world = new World();
            world.ResizeWorld(1, 1);

            Child = new DraggableWorld
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                World = world,
            };
        }
    }
}
