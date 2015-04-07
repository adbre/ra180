using System;
using Ra180.Devices.Dart380;

namespace Ra180.Programs
{
    public abstract class Dart380Program : ProgramBase<Dart380>
    {
        private Action _action;
        private string _key;

        protected Dart380Program(Dart380 device)
            : base(device, device.SmallDisplay)
        {
            LargeDisplay = new DisplayWriter(device.LargeDisplay);
            SmallDisplay = new DisplayWriter(device.SmallDisplay);

            Next(Execute);
        }

        protected abstract void Execute();

        public DisplayWriter LargeDisplay { get; set; }
        public DisplayWriter SmallDisplay { get; set; }

        public string Key
        {
            get { return _key ?? ""; }
            private set { _key = value; }
        }

        protected void Next(Action action)
        {
            Key = "";
            _action = action;
            action();
        }

        public override bool SendKey(string key)
        {
            Key = key;
            _action();
            Key = "";
            return true;
        }

        public override void UpdateDisplay()
        {
            if (IsClosed)
            {
                LargeDisplay.Clear();
                SmallDisplay.Clear();
                base.UpdateDisplay();
                return;
            }

            LargeDisplay.Refresh();
            SmallDisplay.Refresh();
        }
    }
}