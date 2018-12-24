using osu.Framework.Input.Events;
using osuTK.Input;

namespace arbor.Game.Screens
{
    public class ArborMenuScreen : ArborScreen
    {
        protected override bool OnKeyDown(KeyDownEvent keyDownEvent)
        {
            if (keyDownEvent.Repeat || !IsCurrentScreen) return false;

            switch (keyDownEvent.Key)
            {
                case Key.Escape:
                    Exit();
                    return true;
            }

            return base.OnKeyDown(keyDownEvent);
        }
    }
}
