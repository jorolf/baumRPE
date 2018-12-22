using System.Linq;
using eden.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using OpenTK;

namespace eden.Game.Screens.MapEditor.MapWindows
{
    public class NewWorldWindow : MapWindow<World>
    {
        private TextBox filenameBox, widthBox, heightBox;
        private Dropdown<string> tileAtlasDropdown;
        private Story story;
        private NewTileAtlasWindow newTileAtlasWindow;
        public const string WORLD_EXTENSION = ".world";

        public NewTileAtlasWindow NewTileAtlasWindow
        {
            get => newTileAtlasWindow;
            set
            {
                newTileAtlasWindow = value;
                newTileAtlasWindow.OnSubmit += s => { tileAtlasDropdown.AddDropdownItem(s, s); };
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
                    Items = loaderStory?.AtlasFiles.ToDictionary(file =>file),
                    RelativeSizeAxes = Axes.X
                }),
            };

            Add(textBoxContainer);

            tileAtlasDropdown.AddDropdownItem("Select a tile atlas", "SELECT");
            tileAtlasDropdown.AddDropdownItem("Create new tile atlas", "CREATE");
            tileAtlasDropdown.Current.Value = "SELECT";
            tileAtlasDropdown.Current.ValueChanged += value =>
            {
                if (value == "CREATE")
                {
                    NewTileAtlasWindow?.Show();
                    tileAtlasDropdown.Current.Value = "SELECT";
                }
            };
        }

        protected override World SubmitParam
        {
            get
            {
                World world = new World
                {
                    AtlasPath = tileAtlasDropdown.Current,
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
                   && int.TryParse(widthBox.Text, out var _)
                   && int.TryParse(heightBox.Text, out var _)
                   && tileAtlasDropdown.Current != "SELECT"
                   && !story.WorldFiles.Contains(createFilename(filenameBox.Text));
        }

        protected override void ResetValues()
        {
            filenameBox.Text = string.Empty;
            widthBox.Text = string.Empty;
            heightBox.Text = string.Empty;
            tileAtlasDropdown.Current.Value = "SELECT";
        }

        private string createFilename(string filename)
        {
            if (filename.EndsWith(WORLD_EXTENSION))
                return filename;

            return filename + WORLD_EXTENSION;
        }
    }
}
