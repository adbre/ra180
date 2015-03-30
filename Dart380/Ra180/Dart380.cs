﻿namespace Ra180
{
    public class Dart380 : Dart380Base
    {
        private readonly ISynchronizationContext _synchronizationContext;
        private object _clearDisplayToken;
        private Dart380Mod _mod;
        private bool _isOnline;
        private bool _hasData;

        public Dart380(ISynchronizationContext synchronizationContext)
        {
            _synchronizationContext = synchronizationContext;
            _synchronizationContext.Repeat(OnEachSecond, 1000);
        }

        private void OnEachSecond()
        {
            if (IsOnline)
                RefreshDisplay();
        }

        public override bool IsOnline
        {
            get { return _isOnline; }
        }

        public override bool IsPoweredOn
        {
            get { return Mod != Dart380Mod.FR; }
        }

        public Dart380Mod Mod
        {
            get { return _mod; }
        }

        protected override void OnKeyBEL()
        {
            SmallDisplay.ChangeBrightness();
            LargeDisplay.ChangeBrightness();
        }

        protected override bool OnKey(string key)
        {
            if (!base.OnKey(key))
                return false;

            if (_clearDisplayToken != null)
            {
                _synchronizationContext.Cancel(_clearDisplayToken);
                _clearDisplayToken = null;
            }

            return true;
        }

        protected override void OnKeyReset()
        {
            _hasData = false;
            RunSelfTest();
        }

        protected override bool HandleModKey(string key)
        {
            Dart380Mod mod;
            if (!Dart380Key.TryParseMod(key, out mod))
                return false;

            SetMod(mod);
            return true;
        }

        protected override bool HandleSystemKey(string key)
        {
            return false;
        }

        protected override Ra180Program CreateProgram(string key)
        {
            var program = CreateRa180Program(key);
            if (program != null)
                return program;

            return null;
        }

        private Ra180Program CreateRa180Program(string key)
        {
            if (!IsFjärr())
            {
                SmallDisplay.SetText("EJ FJÄRR");
                ScheduleRefreshDisplay();

                return null;
            }

            switch (key)
            {
                case Ra180Key.EFF: return new Ra180EffProgram(Ra180, SmallDisplay);

                case Ra180Key.TID: return new Ra180TidProgram(Ra180, SmallDisplay);
                case Ra180Key.KDA: return new Ra180KdaProgram(Ra180, SmallDisplay);
                case Ra180Key.RDA: return new Ra180RdaProgram(Ra180, SmallDisplay, true);
                case Ra180Key.NYK: return new Ra180NykProgram(Ra180, SmallDisplay);

                default:
                    return null;
            }
        }

        private void ScheduleRefreshDisplay()
        {
            _clearDisplayToken = _synchronizationContext.Schedule(() =>
            {
                RefreshDisplay();
                _clearDisplayToken = null;
            }, 1500);
        }

        private bool IsFjärr()
        {
            return Ra180 != null && Ra180.Mod != Ra180Mod.FR && Ra180.IsOnline;
        }

        private void SetMod(Dart380Mod newMod)
        {
            if (newMod == _mod)
                return;

            if (newMod == Dart380Mod.FR)
            {
                Shutdown();
                return;
            }

            if (_mod == Dart380Mod.FR)
                Start();

            _mod = newMod;
        }

        private void Start()
        {
            RunSelfTest();
        }

        private void RunSelfTest()
        {
            _isOnline = false;

            var selftest = new SelfTest(_synchronizationContext, SmallDisplay)
            {
                Abort = () => _mod == Dart380Mod.FR,
                IsNOLLST = () => !_hasData,
                Complete = () =>
                {
                    RefreshDisplay();
                    _isOnline = true;
                    _hasData = true;
                }
            };

            selftest.Start();
        }

        private void Shutdown()
        {
            ClearDisplay();
            _mod = Dart380Mod.FR;
            _isOnline = false;
        }
    }
}