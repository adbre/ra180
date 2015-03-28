using System;

namespace Ra180
{
    public interface ISynchronizationContext
    {
        object Repeat(Action callback, int interval);
        object Schedule(Action callback, int delay);
        void Cancel(object id);
    }
}