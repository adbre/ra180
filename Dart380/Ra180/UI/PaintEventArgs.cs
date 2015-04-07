using System;

namespace Ra180.UI
{
    public class PaintEventArgs : EventArgs
    {
        private readonly ICanvas _canvas;

        public PaintEventArgs(ICanvas canvas)
        {
            if (canvas == null) throw new ArgumentNullException("canvas");
            _canvas = canvas;
        }

        public ICanvas Canvas { get { return _canvas; } }
    }
}