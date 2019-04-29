using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace arbor.Game.Graphics.UserInterface
{
    public class ArborList : ArborMenu, IHasCurrentValue<MenuItem>
    {
        private DrawableMenuItem selectedItem;

        public Color4 SelectedColour = Color4.SlateGray;
        public Color4 UnselectedColour = Color4.DarkSlateGray;

        public ArborList(Direction direction)
            : base(direction, true)
        {
            Current.ValueChanged += e => System.Diagnostics.Debug.WriteLine($"{e.NewValue} was selected");
            Current.ValueChanged += e => select(Children.FirstOrDefault(menuItem => menuItem.Item == e.NewValue) ?? selectedItem);
        }

        protected override DrawableMenuItem CreateDrawableMenuItem(MenuItem item)
        {
            var drawableItem = new ArborListItem(item);
            drawableItem.Clicked = () => select(drawableItem);
            return drawableItem;
        }

        private void select(DrawableMenuItem drawableItem)
        {
            if (selectedItem != null)
                selectedItem.BackgroundColour = UnselectedColour;
            selectedItem = drawableItem;
            Current.Value = drawableItem.Item;
            selectedItem.BackgroundColour = SelectedColour;
        }

        private readonly Bindable<MenuItem> current = new Bindable<MenuItem>();

        public Bindable<MenuItem> Current
        {
            get => current;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                current.UnbindBindings();
                current.BindTo(value);
            }
        }

        private class ArborListItem : DrawableMenuItem
        {
            public Action Clicked;

            public ArborListItem(MenuItem item)
                : base(item)
            {
            }

            protected override bool OnClick(ClickEvent clickEvent)
            {
                Clicked?.Invoke();
                return base.OnClick(clickEvent);
            }
        }
    }
}
