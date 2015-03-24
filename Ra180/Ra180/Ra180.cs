using System;
using System.Collections.Generic;
using System.Globalization;

namespace Ra180
{
    public class Ra180
    {
        internal const int SELFTEST_INTERVAL = 2000;
        internal const int SELFTEST = SELFTEST_INTERVAL * 4;

        private readonly IRa180Network _network;
        private readonly ISynchronizationContext _synchronizationContext;
        private readonly LedDisplay _display = new LedDisplay(8);

        private readonly Stack<Ra180Program> _programStack = new Stack<Ra180Program>();

        private Ra180Mod _mod;
        private Ra180Volume _volume;
        private bool _online;

        private Ra180Clock _clock;
        private readonly Ra180Data _data = new Ra180Data();

        public Ra180(IRa180Network network, ISynchronizationContext synchronizationContext)
        {
            if (network == null) throw new ArgumentNullException("network");
            if (synchronizationContext == null) throw new ArgumentNullException("synchronizationContext");
            _network = network;
            _network.ReceivedSynk += (sender, args) => _data.CurrentChannelData.Synk = true;
            _synchronizationContext = synchronizationContext;
        }

        public LedDisplay Display { get { return _display; } }

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

        public void SendKeys(string keys)
        {
            if (keys.Contains(Ra180Key.NOLLST))
            {
                SendKey(Ra180Key.NOLLST);
                return;
            }

            foreach (var character in keys)
            {
                SendKey(character.ToString(CultureInfo.InvariantCulture));
            }
        }

        public void SendKeys(params string[] keys)
        {
            if (string.Join("", keys).Contains(Ra180Key.NOLLST))
            {
                SendKey(Ra180Key.NOLLST);
                return;
            }

            foreach (var key in keys)
                SendKey(key);
        }

        public void SendKey(string key)
        {
            if (HandleSystemEvents(key))
            {
                RefreshDisplay();
                return;
            }

            SendKeyToPrograms(key);
        }

        private bool HandleSystemEvents(string key)
        {
            Ra180Mod newMod;
            if (Ra180Key.TryParseMod(key, out newMod))
            {
                SetMod(newMod);
                return true;
            }

            if (_mod == Ra180Mod.FR)
                return true;

            if (key == Ra180Key.NOLLST)
            {
                Reset();
                return true;
            }

            if (key == Ra180Key.BEL)
            {
                _display.ChangeBrightness();
                return true;
            }

            Ra180Channel channel;
            if (Ra180Key.TryParseChannel(key, out channel))
            {
                _data.Channel = channel;
                return true;
            }

            if (!_online)
                return true;

            return false;
        }

        private void SendKeyToPrograms(string key)
        {
            if (SendKeyToCurrentProgram(key))
                return;

            StartNewProgram(key);
        }

        private bool SendKeyToCurrentProgram(string key)
        {
            var program = GetCurrentProgram();
            if (program == null)
                return false;

            program.SendKey(key);
            program.Display();
            return true;
        }

        private Ra180Program GetCurrentProgram()
        {
            if (_programStack.Count == 0)
                return null;

            return _programStack.Peek();
        }

        private void StartNewProgram(string key)
        {
            var program = CreateProgram(key);
            if (program == null)
                return;

            _programStack.Push(program);
            program.Closed += (sender, eventArgs) => _programStack.Pop();
            program.Display();
        }

        private Ra180Program CreateProgram(string key)
        {
            switch (key)
            {
                case Ra180Key.TID: return new Ra180TidProgram(this);
                case Ra180Key.RDA: return new Ra180RdaProgram(this);
                case Ra180Key.DTM: return new Ra180DtmProgram(this);
                case Ra180Key.KDA: return new Ra180KdaProgram(this);
                case Ra180Key.NYK: return new Ra180NykProgram(this);

                default:
                    return null;
            }
        }

        private void Reset()
        {
            RunSelfTest();
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
            Display.SetText("TEST");
            _synchronizationContext.Schedule(() =>
            {
                if (_mod == Ra180Mod.FR)
                    return;

                Display.SetText("TEST OK");
                _synchronizationContext.Schedule(() =>
                {
                    if (_mod == Ra180Mod.FR)
                        return;

                    Display.SetText("NOLLST");
                    _synchronizationContext.Schedule(() =>
                    {
                        Display.Clear();

                        if (_clock == null)
                            _clock = new Ra180Clock(this, _synchronizationContext);

                        _online = true;
                    }, SELFTEST_INTERVAL);
                }, SELFTEST_INTERVAL);
            }, SELFTEST_INTERVAL);
        }

        public void RefreshDisplay()
        {
            if (!_online)
                return;

            var program = GetCurrentProgram();
            if (program != null)
            {
                program.Display();
                return;
            }

            Display.Clear();
        }
    }

    internal class Ra180NykProgram : Ra180MenuProgram
    {
        public Ra180NykProgram(Ra180 ra180) : base(ra180)
        {
            Title = "NYK";

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "NYK",
                CanEdit = () =>
                {
                    var any = Ra180.Data.CurrentChannelData.NYK;
                    var pny = Ra180.Data.CurrentChannelData.PNY;

                    return any != null || pny != null;
                },
                GetValue = () =>
                {
                    var any = Ra180.Data.CurrentChannelData.NYK;
                    if (any == null)
                        return "###";

                    return any.Checksum;
                }
            });
        }
    }
}