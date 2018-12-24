using arbor.Game.Gameplay;
using arbor.Game.IO;
using arbor.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK.Input;

namespace arbor.Game.Screens
{
    /// <summary>
    /// Will load the game
    /// </summary>
    public class GameScreen : ArborScreen
    {
        public GameScreen()
        {
            AlwaysPresent = true;
        }

        [BackgroundDependencyLoader]
        private void load(Story story, JsonStore jsonStore)
        {
            Children = new Drawable[]
            {
                jsonStore.Deserialize<World>(story.SpawnWorldFile),
                new ArborUI
                {
                    Depth = float.MinValue + 1,
                }
            };
        }

        public void Pause()
        {

        }

        protected override bool OnKeyDown(KeyDownEvent keyDownEvent)
        {
            if (keyDownEvent.Repeat) return false;

            if (keyDownEvent.Key == Key.Escape)
                Pause();
            return base.OnKeyDown(keyDownEvent);
        }
    }
}
