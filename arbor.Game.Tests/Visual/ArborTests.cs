using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osuTK.Graphics;

namespace arbor.Game.Tests.Visual
{
    public class ArborTests : ArborBaseGame
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Gray
            });
            Add(new TestBrowser(@"arbor"));
        }
    }
}
