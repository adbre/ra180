using System;
using System.Threading;
using System.Threading.Tasks;

namespace C42A.Ra180.Infrastructure
{
    public class TaskFactory : ITaskFactory
    {
        public void StartNew(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            Task.Factory.StartNew(action);
        }

        public void Wait(TimeSpan delay)
        {
            Thread.Sleep(delay);
        }
    }
}