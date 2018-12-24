using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace arbor.Game.Screens.MapEditor.MapWindows
{
    public class NewTileAtlasWindow : MapWindow<string>
    {
        public const string ATLAS_EXTENSIONS = ".atlas";

        private TextBox filenameBox;
        private Story story;

        [BackgroundDependencyLoader(true)]
        private void load(Story story)
        {
            this.story = story;
            Title = "New tile atlas";
            SubmitText = "Create new tile atlas";

            Add(new NamedContainer("Filename and path: ", filenameBox = new TextBox
            {
                RelativeSizeAxes = Axes.X,
                Height = 20,
                OnCommit = (sender, text) => Submit(),
                ReleaseFocusOnCommit = false,
            })
            {
                RelativeSizeAxes = Axes.None,
                Width = 400
            });
        }

        protected override string SubmitParam => filenameBox.Text + ATLAS_EXTENSIONS;

        protected override bool ValuesValid()
        {
            return !string.IsNullOrEmpty(filenameBox.Text) && (!story?.AtlasFiles.Contains(filenameBox.Text + ATLAS_EXTENSIONS) ?? false);
        }

        protected override void ResetValues()
        {
            filenameBox.Text = string.Empty;
        }
    }
}
