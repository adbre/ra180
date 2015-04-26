using System;
using System.Collections.Generic;
using Ra180.Programs;
using Ra180.UI;

namespace Ra180.Devices.Dart380
{
    public class Dart380 : Dart380Base
    {
        private readonly ISynchronizationContext _synchronizationContext;
        private object _clearDisplayToken;
        private Dart380Mod _mod;
        private bool _isOnline;
        private Dart380Data _dartData;

        private object _anslTvåtråd;
        private object _anslData;
        private object _anslMik2;
        private object _anslMik1; 

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

        public ISynchronizationContext SynchronizationContext
        {
            get { return _synchronizationContext; }
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

        public Dart380Data DartData
        {
            get { return _dartData; }
        }

        public override object Tvåtråd
        {
            get { return _anslTvåtråd; }
            set
            {
                _anslTvåtråd = value;

                if (!IsOnline)
                    return;

                var ra180 = value as Ra180;
                if (ra180 != null)
                    OnRa180Connected(ra180, Fjärranslutning.Tvåtråd);
            }
        }

        public override object Mik2
        {
            get { return _anslMik2; }
            set
            {
                _anslMik2 = value;

                if (!IsOnline)
                    return;

                var ra180 = value as Ra180;
                if (ra180 != null)
                    OnRa180Connected(ra180, Fjärranslutning.Flertråd);
            }
        }

        private enum Fjärranslutning
        {
            Flertråd,
            Tvåtråd
        }

        private void OnRa180Connected(Ra180 ra180, Fjärranslutning anslutning)
        {
            ra180.PlayingAudio += Ra180OnPlayingAudio;

            _dartData.Operatörsmeddelanden.Add(anslutning == Fjärranslutning.Tvåtråd
                ? "ANSL TTR"
                : "ANSL FTR");
        }

        private void Ra180OnPlayingAudio(object sender, AudioFile audioFile)
        {
            PlayLocal(audioFile);
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
            _dartData = null;
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

        protected override ProgramBase CreateProgram(string key)
        {
            switch (key)
            {
                case Dart380Key.DDA: return new Dart380DdaProgram(this, SmallDisplay);
                case Dart380Key.FMT: return new FmtProgram(this);
                case Dart380Key.ISK: return new IskProgram(this);
                case Dart380Key.OPM: return new OpmProgram(this);
            }

            var program = CreateRa180Program(key);
            if (program != null)
                return program;

            return null;
        }

        public void Send(DartMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            var ra180 = Mik2 as Ra180 ?? Tvåtråd as Ra180;
            if (ra180 == null)
            {
                LargeDisplay.CenterText("EJ FJÄRR");
                return;
            }

            message = message.Clone();
            var args = new MessageEventArgs(message.ToStringArray());
            LargeDisplay.CenterText("SÄNDER");
            ra180.Radio.SendDataMessage(args, () =>
            {
                DartData.Messages.Avs.Add(message);
                LargeDisplay.CenterText("SÄNT");
            });
        }

        private ProgramBase CreateRa180Program(string key)
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
                IsNOLLST = () => _dartData == null,
                Complete = () =>
                {
                    RefreshDisplay();
                    _isOnline = true;
                    _dartData = new Dart380Data();
                    _dartData.Operatörsmeddelanden.ItemAdded += (sender, args) => PlayOPM();

                    Tvåtråd = Tvåtråd;
                    Mik1 = Mik1;
                    Mik2 = Mik2;
                }
            };

            selftest.Start();
        }

        private void PlayOPM()
        {
            if (!_dartData.Operatörsmeddelandeton)
                return;

            Play(AudioFile.OPM);
        }

        private void Play(AudioFile file)
        {
            var ra180 = Ra180;
            if (ra180 != null)
                ra180.PlayAudio(file);
            else
                PlayLocal(file);
        }

        private void PlayLocal(AudioFile file)
        {
            var mik1 = Mik1 as IAudio;
            var mik2 = Mik2 as IAudio;

            if (mik1 != null)
                mik1.Play(file);

            if (mik2 != null)
                mik2.Play(file);
        }

        private void Shutdown()
        {
            ClearDisplay();
            _mod = Dart380Mod.FR;
            _isOnline = false;
        }
    }
}