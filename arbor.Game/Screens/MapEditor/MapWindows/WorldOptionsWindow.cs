using System;
using arbor.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace arbor.Game.Screens.MapEditor.MapWindows
{
    public class WorldOptionsWindow : MapWindow<Action<World>>
    {
        private Story story;
        private Dropdown<string> worldDropdown;

        [BackgroundDependencyLoader]
        private void load(Story story)
        {
            this.story = story;
            Title = "World Options";
            SubmitText = "Save";

            Add(new FillFlowContainer
            {
                Width = 400,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 3),
                Children = new Drawable[]
                {
                    new NamedContainer("Tile atlas", worldDropdown = new BasicDropdown<string>
                    {
                        RelativeSizeAxes = Axes.X,
                        Items = story.AtlasFiles
                    })
                },
            });
        }

        public override void Show()
        {
            worldDropdown.Items = story.AtlasFiles;
            base.Show();
        }

        protected override Action<World> SubmitParam => world =>
        {
            world.AtlasPath = worldDropdown.Current;
        };

        protected override bool ValuesValid() => true;

        protected override void ResetValues()
        { }
    }
}
