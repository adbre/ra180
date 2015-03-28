using System;

namespace Ra180
{
    public class Ra180Band : ChangeableBase
    {
        private short _start;
        private short _end;

        public Ra180Band()
        {
            Start = 90;
            End = 00;
        }

        public short Start
        {
            get { return _start; }
            set
            {
                if (_start == value) return;
                _start = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public short End
        {
            get { return _end; }
            set
            {
                if (_end == value) return;
                _end = value;
                OnChanged(EventArgs.Empty);
            }
        }
    }
}