using System;

namespace Ra180.UI
{
    public class SizeChangedEventArgs : EventArgs
    {
        private readonly float _height;
        private readonly float _width;

        public SizeChangedEventArgs(float width, float height)
        {
            _height = height;
            _width = width;
        }

        public float Width
        {
            get { return _width; }
        }

        public float Height
        {
            get { return _height; }
        }
    }
}