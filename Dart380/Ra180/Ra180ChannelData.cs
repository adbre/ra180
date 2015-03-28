using System;

namespace Ra180
{
    public class Ra180ChannelData : ChangeableBase
    {
        private int _fr;
        private readonly Ra180Band _bd1;
        private readonly Ra180Band _bd2;
        private bool _isKlarDisabled;
        private bool _synk;
        private Ra180DataKey _pny;
        private Ra180DataKey _nyk;

        public Ra180ChannelData()
        {
            FR = 00000;
            _bd1 = new Ra180Band();
            _bd2 = new Ra180Band();

            _bd1.Changed += (sender, args) => OnChanged(args);
            _bd2.Changed += (sender, args) => OnChanged(args);
        }

        public Ra180ChannelData(int fr) : this()
        {
            FR = fr;
        }

        public int FR
        {
            get { return _fr; }
            set
            {
                if (_fr == value) return;
                _fr = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public Ra180Band BD1
        {
            get { return _bd1; }
        }

        public Ra180Band BD2
        {
            get { return _bd2; }
        }

        public bool IsKLARDisabled
        {
            get { return _isKlarDisabled; }
            set
            {
                if (_isKlarDisabled == value) return;
                _isKlarDisabled = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public bool Synk
        {
            get { return _synk; }
            set
            {
                if (_synk == value) return;
                _synk = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public Ra180DataKey PNY
        {
            get { return _pny; }
            set
            {
                _pny = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public Ra180DataKey NYK
        {
            get { return _nyk; }
            set
            {
                _nyk = value;
                OnChanged(EventArgs.Empty);
            }
        }
    }
}