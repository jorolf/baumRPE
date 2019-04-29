using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;

namespace arbor.Game.Screens
{
    public class ArborMenuScreen : ArborScreen
    {
        protected override bool OnKeyDown(KeyDownEvent keyDownEvent)
        {
            if (keyDownEvent.Repeat || !this.IsCurrentScreen()) return false;

            switch (keyDownEvent.Key)
            {
                case Key.Escape:
                    this.Exit();
                    return true;
            }

            return base.OnKeyDown(keyDownEvent);
        }
    }
}
