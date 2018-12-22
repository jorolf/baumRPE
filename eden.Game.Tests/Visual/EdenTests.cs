using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using OpenTK.Graphics;

namespace eden.Game.Tests.Visual
{
    public class EdenTests : EdenBaseGame
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Gray
            });
            Add(new TestBrowser(@"eden"));
        }
    }
}
