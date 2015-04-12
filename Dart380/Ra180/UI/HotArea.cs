using System;

namespace Ra180.UI
{
    public class HotArea
    {
        public HotArea()
        {
        }

        public HotArea(int x, int y, int w, int h, Action onTouch)
            : this(new Rectangle(x, y, x+w, y+h), onTouch)
        {
            
        }

        public HotArea(Rectangle originalRectangle, Action clicked)
        {
            OriginalRect = originalRectangle;
            Rectangle = originalRectangle;
            Clicked += (sender, args) => clicked();
        }

        public event EventHandler Clicked;
        public Rectangle OriginalRect { get; set; }
        public Rectangle Rectangle { get; set; }

        public void Scale(float scale, Rectangle area)
        {
            Rectangle = OriginalRect.Scale(scale).PlaceInside(area);
        }

        public virtual void OnTouchEventDown()
        {
            var handler = Clicked;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public virtual void OnTouchEventUp()
        {
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