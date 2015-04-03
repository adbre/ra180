using System;

namespace Ra180.Programs
{
    public abstract class ProgramBase
    {
        protected ProgramBase(Ra180Device device, Ra180Display display)
        {
            Device = device;
            Display = display;
        }

        protected ProgramBase()
        {
        }

        public string Title { get; set; }

        public Ra180Device Device { get; set; }

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

    public abstract class ProgramBase<TDevice> : ProgramBase
        where TDevice : Ra180Device
    {
        protected ProgramBase(TDevice device, Ra180Display display) : base(device, display)
        {
        }

        protected ProgramBase()
        {
        }

        public new TDevice Device
        {
            get { return (TDevice)base.Device; }
            set { base.Device = value; }
        }
    }
}