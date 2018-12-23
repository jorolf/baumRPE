using System;
using System.Collections.Generic;
using eden.Game.Graphics.Containers;
using eden.Game.Screens.MapEditor;
using eden.Game.Screens.MapEditor.MapWindows;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osuTK.Graphics;

namespace eden.Game.Tests.Visual
{
    public class TestCaseWindow : TestCase
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
            {typeof(EdenWindow), typeof(NewWorldWindow), typeof(NewTileAtlasWindow), typeof(MapWindow<>)};

        public TestCaseWindow()
        {
            EdenWindow worldWindow;
            NewTileAtlasWindow atlasWindow;

            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Gray
            });
            Add(atlasWindow = new NewTileAtlasWindow
            {
                State = Visibility.Visible,
                Y = 300
            });
            Add(worldWindow = new NewWorldWindow
            {
                State = Visibility.Visible,
                NewTileAtlasWindow = atlasWindow,
            });

            AddStep("show world window", worldWindow.Show);
            AddStep("hide world window", worldWindow.Hide);
            AddStep("show atlas window", atlasWindow.Show);
            AddStep("hide atlas window", atlasWindow.Hide);
        }
    }
}
