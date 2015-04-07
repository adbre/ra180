using System;

namespace Ra180.UI
{
    public class MotionEventArgs : EventArgs
    {
        private readonly float _x;
        private readonly float _y;
        private readonly MotionEventActions _action;

        public MotionEventArgs(float x, float y, MotionEventActions action)
        {
            _x = x;
            _y = y;
            _action = action;
        }

        public float X { get { return _x; } }
        public float Y { get { return _y; } }

        public MotionEventActions Action { get { return _action; } }
    }
}