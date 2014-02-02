using System;

namespace C42A.Ra180.Infrastructure
{
    public interface ITaskFactory
    {
        void StartNew(Action action);
        void Wait(TimeSpan delay);
    }
}