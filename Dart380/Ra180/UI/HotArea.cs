using System;

namespace Ra180.UI
{
    public class HotArea
    {
        public HotArea()
        {
        }

        public HotArea(Rectangle originalRectangle, Action callback)
        {
            OriginalRect = originalRectangle;
            Callback = callback;
        }

        public Action Callback { get; set; }
        public Rectangle OriginalRect { get; set; }
        public Rectangle Rectangle { get; set; }

        public void Scale(float scale, Rectangle area)
        {
            Rectangle = OriginalRect.Scale(scale).PlaceInside(area);
        }

        protected virtual void OnTouchEventDown()
        {
            var handler = Callback;
            if (handler != null)
                handler();
        }

        protected virtual void OnTouchEventUp()
        {
        }

        public virtual void OnTouchEvent(MotionEventArgs e)
        {
            var contains = Rectangle.Contains(e.X, e.Y);
            if (contains && e.Action == MotionEventActions.Down)
                OnTouchEventDown();
            else
                OnTouchEventUp();
        }
    }

    public class Dart380Control : HotArea
    {
        public Dart380Control(float x, float y, float w, float h)
        {
            OriginalRect = new Rectangle(x, y, x + w, y + h);
        }
    }
}