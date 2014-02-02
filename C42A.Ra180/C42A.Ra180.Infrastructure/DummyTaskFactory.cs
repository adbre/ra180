using System;

namespace C42A.Ra180.Infrastructure
{
    public class DummyTaskFactory : ITaskFactory
    {
        public void StartNew(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            action();
        }

        public void Wait(TimeSpan delay)
        {
            
        }
    }
}