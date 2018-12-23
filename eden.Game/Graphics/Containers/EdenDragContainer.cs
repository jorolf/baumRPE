using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK.Input;

namespace eden.Game.Graphics.Containers
{
    public class EdenDragContainer : Container
    {
        private bool dragging;

        protected virtual bool ShouldDrag(MouseEvent mouseEvent) => mouseEvent.IsPressed(MouseButton.Button1);

        protected override bool OnMouseDown(MouseDownEvent mouseDownEvent) => dragging = ShouldDrag(mouseDownEvent);

        protected override bool OnMouseMove(MouseMoveEvent mouseMoveEvent)
        {
            if(dragging)
                Position += mouseMoveEvent.Delta;
            return dragging;
        }

        protected override bool OnMouseUp(MouseUpEvent mouseUpEvent) => !(dragging = ShouldDrag(mouseUpEvent));
    }
}
