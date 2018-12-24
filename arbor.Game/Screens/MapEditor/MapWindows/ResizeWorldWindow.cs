using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace arbor.Game.Screens.MapEditor.MapWindows
{
    public class ResizeWorldWindow : MapWindow<Vector2I>
    {
        private TextBox heightBox;
        private TextBox widthBox;

        [BackgroundDependencyLoader]
        private void load()
        {
            Title = "World Size";
            SubmitText = "Resize world";

            var textBoxContainer = new FillFlowContainer
            {
                Width = 400,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 3),
            };

            textBoxContainer.Children = new Drawable[]
            {
                new NamedContainer("Width: ", widthBox = new TextBox
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 20,
                    TabbableContentContainer = textBoxContainer,
                }),
                new NamedContainer("Height: ", heightBox = new TextBox
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 20,
                    TabbableContentContainer = textBoxContainer,
                    OnCommit = (_, __) => Submit(),
                }),
            };

            Add(textBoxContainer);
        }

        protected override Vector2I SubmitParam => new Vector2I(int.Parse(widthBox.Text), int.Parse(heightBox.Text));

        protected override bool ValuesValid() => int.TryParse(widthBox.Text, out int _) && int.TryParse(heightBox.Text, out int _);

        protected override void ResetValues()
        {
            widthBox.Text = string.Empty;
            heightBox.Text = string.Empty;
        }
    }
}
