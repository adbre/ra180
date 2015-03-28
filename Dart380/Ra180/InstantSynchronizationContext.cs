using System;

namespace Ra180
{
    public class InstantSynchronizationContext : ISynchronizationContext
    {
        public object Repeat(Action callback, int interval)
        {
            return null;
        }

        public object Schedule(Action callback, int delay)
        {
            callback();
            return null;
        }

        public void Cancel(object id)
        {
        }
    }
}