using eden.Game.Gameplay;
using eden.Game.IO;
using eden.Game.Worlds;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using OpenTK.Input;

namespace eden.Game.Screens
{
    /// <summary>
    /// Will load the game
    /// </summary>
    public class GameScreen : EdenScreen
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
                new EdenUI
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
