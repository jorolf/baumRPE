using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace arbor.Game.Screens.MapEditor.MapWindows
{
    public class StoryOptionsWindow : MapWindow<Action>
    {
        private Dropdown<string> spawnWorldDropdown;
        private TextBox storyNameTextBox;
        private Story story;

        [BackgroundDependencyLoader]
        private void load(Story story)
        {
            Title = "Story Options";
            SubmitText = "OK";

            Content.AutoSizeAxes = Axes.Y;
            Content.Width = 300;

            this.story = story;
            Children = new[]
            {
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Children = new[]
                    {
                        new NamedContainer("Spawn World", spawnWorldDropdown = new BasicDropdown<string>
                        {
                            RelativeSizeAxes = Axes.X,
                            Items = story.WorldFiles
                        }),
                        new NamedContainer("Story Name", storyNameTextBox = new TextBox
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 20,
                            OnCommit = (_, __) => Submit()
                        })
                    }
                }
            };

            ResetValues();
        }

        protected override Action SubmitParam => () =>
        {
            story.SpawnWorldFile = spawnWorldDropdown.Current;
            story.Name = storyNameTextBox.Text;
        };

        protected override bool ValuesValid() => !string.IsNullOrWhiteSpace(storyNameTextBox.Text);

        protected override void ResetValues()
        {
            storyNameTextBox.Text = story.Name;
            spawnWorldDropdown.Current.Value = story.SpawnWorldFile ?? story.WorldFiles.First();
        }
    }
}
