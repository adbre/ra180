using System;

namespace Ra180
{
    public abstract class Ra180Program
    {
        protected Ra180Program(Ra180 ra180, Ra180Display display)
        {
            Ra180 = ra180;
            Display = display;
        }

        protected Ra180Program()
        {
        }

        public string Title { get; set; }

        public Ra180 Ra180 { get; set; }
        public Ra180Display Display { get; set; }

        public bool IsClosed { get; private set; }

        public virtual bool Disabled { get { return false; }}

        public virtual bool SendKey(string key)
        {
            return false;
        }

        public event EventHandler Closed;

        public virtual void UpdateDisplay()
        {
            if (IsClosed)
            {
                Display.Clear();
                return;
            }

            var title = Title ?? string.Empty;
            if (title.Length == 3)
                Display.SetText(string.Format("  ({0})", title));
            else
                Display.SetText(string.Format("({0})", title));
        }

        public virtual void Close()
        {
            Display.Clear();
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