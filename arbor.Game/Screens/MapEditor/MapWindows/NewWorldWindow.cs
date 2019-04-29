using arbor.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace arbor.Game.Screens.MapEditor.MapWindows
{
    public class NewWorldWindow : MapWindow<World>
    {
        private TextBox filenameBox, widthBox, heightBox;
        private Dropdown<string> tileAtlasDropdown;
        private Story story;
        private NewTileAtlasWindow newTileAtlasWindow;
        public const string WORLD_EXTENSION = ".world";
        private const string select = "Select a tile atlas", create = "Create a tile atlas";

        public NewTileAtlasWindow NewTileAtlasWindow
        {
            get => newTileAtlasWindow;
            set
            {
                newTileAtlasWindow = value;
                newTileAtlasWindow.OnSubmit += s => tileAtlasDropdown.AddDropdownItem(s);
            }
        }

        [BackgroundDependencyLoader(true)]
        private void load(Story loaderStory)
        {
            story = loaderStory;
            Title = "New world";
            SubmitText = "Create new world";

            var textBoxContainer = new FillFlowContainer
            {
                Width = 400,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 3),
            };

            textBoxContainer.Children = new Drawable[]
            {
                new NamedContainer("Filename and path: ", filenameBox = new TextBox
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 20,
                    TabbableContentContainer = textBoxContainer,
                }),
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
                }),
                new NamedContainer("Tile Atlas: ", tileAtlasDropdown = new BasicDropdown<string>
                {
                    Items = loaderStory?.AtlasFiles,
                    RelativeSizeAxes = Axes.X
                }),
            };

            Add(textBoxContainer);

            tileAtlasDropdown.AddDropdownItem(select);
            tileAtlasDropdown.AddDropdownItem(create);
            tileAtlasDropdown.Current.Value = select;
            tileAtlasDropdown.Current.ValueChanged += e =>
            {
                if (e.NewValue == create)
                {
                    NewTileAtlasWindow?.Show();
                    tileAtlasDropdown.Current.Value = select;
                }
            };
        }

        protected override World SubmitParam
        {
            get
            {
                World world = new World
                {
                    AtlasPath = tileAtlasDropdown.Current.Value,
                    WorldName = filenameBox.Text + WORLD_EXTENSION
                };
                world.ResizeWorld(int.Parse(widthBox.Text), int.Parse(heightBox.Text));
                story.WorldFiles.Add(world.WorldName);
                return world;
            }
        }

        protected override bool ValuesValid()
        {
            return !string.IsNullOrEmpty(filenameBox.Text)
                   && int.TryParse(widthBox.Text, out _)
                   && int.TryParse(heightBox.Text, out _)
                   && tileAtlasDropdown.Current.Value != select
                   && !story.WorldFiles.Contains(createFilename(filenameBox.Text));
        }

        protected override void ResetValues()
        {
            filenameBox.Text = string.Empty;
            widthBox.Text = string.Empty;
            heightBox.Text = string.Empty;
            tileAtlasDropdown.Current.Value = select;
        }

        private string createFilename(string filename)
        {
            if (filename.EndsWith(WORLD_EXTENSION))
                return filename;

            return filename + WORLD_EXTENSION;
        }
    }
}
