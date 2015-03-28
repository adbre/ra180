using System;

namespace Ra180
{
    public class SelfTest
    {
        public const int INTERVAL = 2000;

        private readonly ISynchronizationContext _synchronizationContext;
        private readonly Ra180Display _display;

        public SelfTest(ISynchronizationContext synchronizationContext, Ra180Display display)
        {
            if (synchronizationContext == null) throw new ArgumentNullException("synchronizationContext");
            if (display == null) throw new ArgumentNullException("display");
            _synchronizationContext = synchronizationContext;
            _display = display;
        }

        public Func<bool> Abort { get; set; }
        public Func<bool> IsNOLLST { get; set; } 
        public Action Complete { get; set; }

        public void Start()
        {
            _display.SetText("TEST");
            _synchronizationContext.Schedule(TestCompleted, INTERVAL);
        }

        private void TestCompleted()
        {
            if (Abort())
                return;

            _display.SetText("TEST OK");
            _synchronizationContext.Schedule(CheckIfNollst, INTERVAL);
        }

        private void CheckIfNollst()
        {
            if (Abort())
                return;

            if (IsNOLLST())
            {
                _display.SetText("NOLLST");
                _synchronizationContext.Schedule(OnComplete, INTERVAL);
            }
            else
                OnComplete();
        }

        private void OnComplete()
        {
            _display.Clear();
            Complete();
        }
    }
}
