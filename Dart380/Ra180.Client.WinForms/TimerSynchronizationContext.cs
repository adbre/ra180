using System;
using System.Diagnostics;
using System.Timers;

namespace Ra180.Client.WinForms
{
    public class TimerSynchronizationContext : ISynchronizationContext, IDisposable
    {
        private readonly object _padlock = new object();
        private readonly DelayedSynchronizationContext _context = new DelayedSynchronizationContext();
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Timer _timer = new Timer();

        public TimerSynchronizationContext(int interval)
        {
            _timer.Interval = interval;
            _timer.Elapsed += TimerElapsed;
        }

        public TimerSynchronizationContext(TimeSpan interval)
            : this((int)interval.TotalMilliseconds)
        {
        }

        /// <summary>
        /// Initializes a new instance of TimerSynchronizationContext which will elapse every 100 milliseconds.
        /// </summary>
        public TimerSynchronizationContext() : this(100)
        {
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            lock (_padlock)
            {
                if (!_stopwatch.IsRunning)
                    return;

                _context.Tick((int) _stopwatch.ElapsedMilliseconds);
                _stopwatch.Restart();
            }
        }

        public void Start()
        {
            lock (_padlock)
            {
                if (_timer.Enabled) return;

                _stopwatch.Start();
                _timer.Enabled = true;
            }
        }

        public void Stop()
        {
            lock (_padlock)
            {
                if (!_timer.Enabled) return;

                _stopwatch.Stop();
                _stopwatch.Reset();
                _timer.Enabled = false;
            }
        }

        public object Repeat(Action callback, int interval)
        {
            return _context.Repeat(callback, interval);
        }

        public object Schedule(Action callback, int delay)
        {
            return _context.Schedule(callback, delay);
        }

        public void Cancel(object id)
        {
            _context.Cancel(id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
        }
    }
}
