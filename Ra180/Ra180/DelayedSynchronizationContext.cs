using System;
using System.Collections.Generic;
using System.Linq;

namespace Ra180
{
    public class DelayedSynchronizationContext : ISynchronizationContext
    {
        private readonly List<CallbackSchedule> _schedules = new List<CallbackSchedule>();
        private long _now = 0;

        public object Repeat(Action callback, int interval)
        {
            return Schedule(new CallbackSchedule(callback, interval) { Recurring = true });
        }

        public object Schedule(Action callback, int delay)
        {
            return Schedule(new CallbackSchedule(callback, delay));
        }

        public void Cancel(object id)
        {
            var schedule = _schedules.FirstOrDefault(s => s.Id == id);
            if (schedule != null)
                _schedules.Remove(schedule);
        }

        public void Tick(TimeSpan timeSpan)
        {
            Tick((int)timeSpan.TotalMilliseconds);
        }

        public void Tick(int milliseconds)
        {
            var endTime = _now + milliseconds;
            RunScheduledCallbacks(endTime);
            _now = endTime;
        }

        private void RunScheduledCallbacks(long endTime)
        {
            if (_schedules.Count < 1 || _schedules.First().RunAtMillisecond > endTime)
                return;

            do
            {
                var schedule = _schedules[0];
                _now = schedule.RunAtMillisecond;
                _schedules.RemoveAt(0);

                schedule.Callback();

                if (schedule.Recurring)
                    Schedule(schedule);

            } while (_schedules.Count > 0 && _now != endTime && _schedules[0].RunAtMillisecond <= endTime);
        }

        private object Schedule(CallbackSchedule schedule)
        {
            schedule.RunAtMillisecond = _now + schedule.Interval;

            _schedules.Add(schedule);
            _schedules.Sort(delegate(CallbackSchedule a, CallbackSchedule b)
            {
                if (a.RunAtMillisecond > b.RunAtMillisecond) return 1;
                if (a.RunAtMillisecond < b.RunAtMillisecond) return -1;
                return 0;
            });

            return schedule.Id;
        }

        private class CallbackSchedule
        {
            public CallbackSchedule(Action callback, int interval)
            {
                Id = Guid.NewGuid();
                Callback = callback;
                Interval = interval;
            }

            public object Id { get; private set; }
            public long RunAtMillisecond { get; set; }
            public Action Callback { get; set; }
            public int Interval { get; set; }
            public bool Recurring { get; set; }
        }
    }
}