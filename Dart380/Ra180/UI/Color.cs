namespace Ra180.UI
{
    public struct Color
    {
        private readonly int _a;
        private readonly int _r;
        private readonly int _b;
        private readonly int _g;

        public Color(int a, int r, int b, int g)
        {
            _a = a;
            _r = r;
            _b = b;
            _g = g;
        }

        public int A { get { return _a; } }
        public int R { get { return _r; } }
        public int B { get { return _b; } }
        public int G { get { return _g; } }
    }
}