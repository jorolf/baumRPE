using osu.Framework.Input.Events;
using osuTK.Input;

namespace eden.Game.Screens
{
    public class EdenMenuScreen : EdenScreen
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
