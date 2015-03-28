using System;
using System.Linq;

namespace Ra180
{
    public class Ra180Clock
    {
        private readonly Ra180 _ra180;
        private readonly ISynchronizationContext _synchronizationContext;
        private readonly object _token;

        public Ra180Clock(Ra180 ra180, ISynchronizationContext synchronizationContext)
        {
            if (synchronizationContext == null) throw new ArgumentNullException("synchronizationContext");
            _ra180 = ra180;
            _synchronizationContext = synchronizationContext;
            _token = _synchronizationContext.Repeat(OnElapse, 1000);

            Day = 1;
            Month = 1;
        }

        ~Ra180Clock()
        {
            _synchronizationContext.Cancel(_token);
        }

        public event EventHandler Tick;

        protected virtual void OnTick()
        {
            var handler = Tick;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public int Second { get; private set; }
        public int Minute { get; private set; }
        public int Hour { get; private set; }
        public int Day { get; private set; }
        public int Month { get; private set; }

        private void OnElapse()
        {
            Second += 1;

            if (Second >= 60)
            {
                Second = 0;
                Minute++;
            }

            if (Minute >= 60)
            {
                Minute = 0;
                Hour++;
            }

            if (Hour >= 24)
            {
                Hour = 0;
                Day++;
            }

            if (Day >= GetDaysInMonth(Month))
            {
                Day = 1;
                Month++;
            }

            if (Month > 12)
            {
                Day = 1;
                Month = 1;
            }

            OnTick();
        }

        private int GetDaysInMonth(int month)
        {
            switch (month)
            {
                case 1: return 31;
                case 2: return 28;
                case 3: return 31;
                case 4: return 30;
                case 5: return 31;
                case 6: return 30;
                case 7: return 31;
                case 8: return 31;
                case 9: return 30;
                case 10: return 31;
                case 11: return 30;
                case 12: return 31;

                default:
                    throw new ArgumentOutOfRangeException("month");
            }
        }

        public bool TrySetTime(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (text.Length != 6) return false;
            if (!text.All(Char.IsDigit)) return false;

            var hour = int.Parse(text.Substring(0, 2));
            var minute = int.Parse(text.Substring(2, 2));
            var second = int.Parse(text.Substring(4, 2));

            if (hour >= 24) return false;
            if (minute >= 60) return false;
            if (second >= 60) return false;

            Hour = hour;
            Minute = minute;
            Second = second;
            return true;
        }

        public bool TrySetDate(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (text.Length != 4) return false;
            if (!text.All(Char.IsDigit)) return false;

            var month = int.Parse(text.Substring(0, 2));
            var day = int.Parse(text.Substring(2, 2));

            if (month >= 13) return false;
            if (GetDaysInMonth(month) < day) return false;

            Month = month;
            Day = day;
            return true;
        }

        public string GetTime()
        {
            return string.Format("{0:00}{1:00}{2:00}", Hour, Minute, Second);
        }

        public string GetDate()
        {
            return string.Format("{0:00}{1:00}", Month, Day);
        }
    }
}