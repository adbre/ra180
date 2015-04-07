using System;
using Ra180.Programs;

namespace Ra180
{
    public class Ra180 : Ra180Device
    {
        internal const int SELFTEST_INTERVAL = SelfTest.INTERVAL;
        internal const int SELFTEST = SELFTEST_INTERVAL * 4;

        private readonly IRadio _network;
        private readonly ISynchronizationContext _synchronizationContext;
        private readonly Ra180Display _display = new Ra180Display(8);

        private Ra180Mod _mod;
        private Ra180Volume _volume;
        private bool _online;

        private Ra180Clock _clock;
        private readonly Ra180Data _data = new Ra180Data();

        public Ra180(IRadio network, ISynchronizationContext synchronizationContext)
        {
            if (network == null) throw new ArgumentNullException("network");
            if (synchronizationContext == null) throw new ArgumentNullException("synchronizationContext");
            _network = network;
            _network.ReceivedSynk += (sender, args) => _data.CurrentChannelData.Synk = true;
            _synchronizationContext = synchronizationContext;
        }

        public Ra180Display Display { get { return _display; } }

        public override bool IsOnline
        {
            get { return _online; }
        }

        public override bool IsPoweredOn
        {
            get { return Mod != Ra180Mod.FR; }
        }

        public Ra180Mod Mod
        {
            get { return _mod; }
            set { SendKey(Ra180Key.From(value)); }
        }

        public Ra180Channel Channel
        {
            get { return Data.Channel; }
            set { SendKey(Ra180Key.From(value)); }
        }

        public Ra180Volume Volume
        {
            get { return _volume; }
            set { SendKey(Ra180Key.From(value)); }
        }

        internal Ra180Clock Clock
        {
            get { return _clock; }
        }

        internal Ra180Data Data { get { return _data; } }

        public IRadio Radio
        {
            get { return _network; }
        }

        protected override void OnKeyBEL()
        {
            _display.ChangeBrightness();
        }

        protected override void OnKeyReset()
        {
            RunSelfTest();
        }

        protected override bool HandleModKey(string key)
        {
            Ra180Mod newMod;
            if (!Ra180Key.TryParseMod(key, out newMod))
                return false;

            SetMod(newMod);
            return true;
        }

        protected override bool HandleSystemKey(string key)
        {
            if (key == Ra180Key.NOLLST)
            {
                RunSelfTest();
                return true;
            }

            Ra180Channel channel;
            if (Ra180Key.TryParseChannel(key, out channel))
            {
                _data.Channel = channel;
                return true;
            }

            return false;
        }

        protected override ProgramBase CreateProgram(string key)
        {
            switch (key)
            {
                case Ra180Key.TID: return new Ra180TidProgram(this, Display);
                case Ra180Key.RDA: return new Ra180RdaProgram(this, Display);
                case Ra180Key.DTM: return new Ra180DtmProgram(this, Display);
                case Ra180Key.KDA: return new Ra180KdaProgram(this, Display);
                case Ra180Key.NYK: return new Ra180NykProgram(this, Display);

                default:
                    return null;
            }
        }

        protected override void ClearDisplay()
        {
            _display.Clear();
        }

        private void SetMod(Ra180Mod newMod)
        {
            if (newMod == _mod)
                return;

            if (newMod == Ra180Mod.FR)
            {
                Shutdown();
                return;
            }

            if (_mod == Ra180Mod.FR)
                Start();

            _mod = newMod;
        }

        private void Start()
        {
            RunSelfTest();
        }

        private void Shutdown()
        {
            Display.Clear();
            _mod = Ra180Mod.FR;
            _online = false;
        }

        private void RunSelfTest()
        {
            _online = false;

            var selftest = new SelfTest(_synchronizationContext, Display)
            {
                Abort = () => _mod == Ra180Mod.FR,
                IsNOLLST = () => _data.IsNOLLST,
                Complete = () =>
                {
                    Display.Clear();
                    ResumeClock();
                    _online = true;
                }
            };

            selftest.Start();
        }

        private void ResumeClock()
        {
            if (_clock == null)
            {
                _clock = new Ra180Clock(this, _synchronizationContext);
                _clock.Tick += (sender, args) => RefreshDisplay();
            }
        }
    }
}