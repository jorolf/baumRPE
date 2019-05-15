using System;
using System.Collections.Generic;
using arbor.Game.Graphics.Containers;
using arbor.Game.Screens.MapEditor;
using arbor.Game.Screens.MapEditor.MapWindows;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osuTK.Graphics;

namespace arbor.Game.Tests.Visual
{
    public class TestSceneWindow : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
            { typeof(ArborWindow), typeof(NewWorldWindow), typeof(NewTileAtlasWindow), typeof(MapWindow<>) };

        public TestSceneWindow()
        {
            ArborWindow worldWindow;
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
