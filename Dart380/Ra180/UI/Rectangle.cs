namespace Ra180.UI
{
    public struct Rectangle
    {
        private readonly float _left;
        private readonly float _top;
        private readonly float _right;
        private readonly float _bottom;

        public Rectangle(float left, float top, float right, float bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        public float Top { get { return _top; } }
        public float Left { get { return _left; } }
        public float Height { get { return _bottom - _top; } }
        public float Width { get { return _right - _left; } }

        public float Right { get { return _right; } }
        public float Bottom { get { return _bottom; } }

        public float X { get { return _left; } }
        public float Y { get { return _top; } }
    }
}