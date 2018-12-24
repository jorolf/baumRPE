using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace arbor.Game.Screens.MapEditor.MapWindows
{
    public class RenameWorldWindow : MapWindow<string>
    {
        private TextBox nameBox;

        [BackgroundDependencyLoader]
        private void load()
        {
            Title = "World Name";
            SubmitText = "Rename world";

            Content.AutoSizeAxes = Axes.Y;
            Content.Width = 200;

            Add(new NamedContainer("World name: ", nameBox = new TextBox
            {
                RelativeSizeAxes = Axes.X,
                Height = 20,
                OnCommit = (_, __) => Submit()
            }));
        }

        protected override string SubmitParam => nameBox.Text;

        protected override bool ValuesValid() => true;

        protected override void ResetValues()
        {
            nameBox.Text = string.Empty;
        }
    }
}
