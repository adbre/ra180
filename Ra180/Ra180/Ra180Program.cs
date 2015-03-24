using System;

namespace Ra180
{
    public abstract class Ra180Program
    {
        protected Ra180Program(Ra180 ra180)
        {
            Ra180 = ra180;
        }

        protected Ra180Program()
        {
        }

        public string Title { get; set; }

        public Ra180 Ra180 { get; set; }

        public bool IsClosed { get; private set; }

        public virtual bool Disabled { get { return false; }}

        public virtual bool SendKey(string key)
        {
            return false;
        }

        public event EventHandler Closed;

        public virtual void Display()
        {
            if (IsClosed)
            {
                Ra180.Display.Clear();
                return;
            }

            var title = Title ?? string.Empty;
            if (title.Length == 3)
                Ra180.Display.SetTextFormat("  ({0})", title);
            else
                Ra180.Display.SetTextFormat("({0})", title);
        }

        public virtual void Close()
        {
            Ra180.Display.Clear();
            IsClosed = true;
            OnClosed();
        }

        protected virtual void OnClosed()
        {
            EventHandler handler = Closed;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}