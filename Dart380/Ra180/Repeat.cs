using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Ra180
{
    public class Repeat
    {
        private TimeSpan _timeout;
        private TimeSpan _interval;

        public Repeat()
        {
            _timeout = TimeSpan.FromSeconds(90);
        }

        public Action Action { get; set; }
        public Func<bool> Until { get; set; }

        public TimeSpan Interval
        {
            get { return _interval; }
            set
            {
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("value", "Interval may not be less than zero.");

                _interval = value;
            }
        }

        public TimeSpan Timeout
        {
            get { return _timeout; }
            set
            {
                if (value.TotalMilliseconds <= 0 && value != System.Threading.Timeout.InfiniteTimeSpan)
                    throw new ArgumentOutOfRangeException("value", "Timeout must not be less than or equal to zero if not equal Timeout.InfinteTimeSpan");

                _timeout = value;
            }
        }

        public static RepeatBuilder NewBuilder()
        {
            return new RepeatBuilder();
        }

        public void Execute()
        {
            Execute(Action, Until, Interval, Timeout);
        }

        private static void Execute(Action action, Func<bool> until, TimeSpan interval, TimeSpan timeout)
        {
            if (until == null) throw new InvalidOperationException("Until must not be null");

            if (action == null)
                action = (() => { });

            var stopwatch = new Stopwatch();
            var isInfinite = timeout == System.Threading.Timeout.InfiniteTimeSpan;

            if (!isInfinite)
                stopwatch.Start();

            do
            {
                if (until())
                    return;

                action();

                if (interval > TimeSpan.Zero)
                    Task.Delay(interval);

            } while (isInfinite || stopwatch.Elapsed < timeout);

            throw new TimeoutException();
        }
    }

    public class RepeatBuilder
    {
        private readonly Repeat _repeat = new Repeat();

        public RepeatBuilder Action(Action action)
        {
            return Build(() => _repeat.Action = action);
        }

        public RepeatBuilder Until(Func<bool> callback)
        {
            return Build(() => _repeat.Until = callback);
        }

        public RepeatBuilder WithInterval(TimeSpan interval)
        {
            return Build(() => _repeat.Interval = interval);
        }

        public RepeatBuilder WithInterval(int intervalInMilliseconds)
        {
            return WithInterval(TimeSpan.FromMilliseconds(intervalInMilliseconds));
        }

        public RepeatBuilder Infintivly()
        {
            return WithTimeout(Timeout.InfiniteTimeSpan);
        }

        public RepeatBuilder WithTimeout(TimeSpan timeout)
        {
            return Build(() => _repeat.Timeout = timeout);
        }

        public RepeatBuilder WithTimeout(int timeoutInMilliseconds)
        {
            return WithTimeout(TimeSpan.FromMilliseconds(timeoutInMilliseconds));
        }

        public void Start()
        {
            _repeat.Execute();
        }

        private RepeatBuilder Build(Action action)
        {
            action();
            return this;
        }
    }
}
