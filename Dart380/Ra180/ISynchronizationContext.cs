using System;

namespace Ra180
{
    public interface ISynchronizationContext
    {
        object Repeat(Action callback, int interval);
        object Schedule(Action callback, int delay);
        void Cancel(object id);
    }

    public static class SynchronizationContext
    {
        [ThreadStatic]
        private static ISynchronizationContext _current;

        public static ISynchronizationContext Current
        {
            get { return _current; }
            set { _current = value; }
        }
    }
}