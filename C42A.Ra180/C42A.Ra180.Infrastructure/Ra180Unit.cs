﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace C42A.Ra180.Infrastructure
{
    public class Ra180Unit : IDisposable
    {
        private readonly ITaskFactory _taskFactory;
        private readonly IDartTransport _transport;
        private readonly Guid _id = Guid.NewGuid();
        private int _kanal = 1;
        private int _volym = 4;
        private Ra180Menu _currentMenu;
        private Ra180Hemligt _hemligt;
        private Ra180Mod _mod;
        private readonly Timer _timer;
        private bool _isStarted;


        public Ra180Unit(ITaskFactory taskFactory, IDartTransport transport)
        {
            if (taskFactory == null) throw new ArgumentNullException("taskFactory");
            if (transport == null) throw new ArgumentNullException("transport");
            _taskFactory = taskFactory;
            _transport = transport;
            _transport.ReceivedString += ReceiveString;
            Now = new DateTimeOffset(2016, 01, 01, 00, 00, 00, TimeSpan.Zero);

            _timer = new Timer(state => SetNow(Now.AddSeconds(1)), null, -1, -1);
        }

        private void ReceiveString(object sender, string e)
        {
            ReceiveString(e);
        }

        public void Start()
        {
            _isStarted = true;
        }

        public int Kanal { get { return _kanal; }}

        public string RadionätId
        {
            get
            {
                var kanaldata = Kanaldata;
                var frekvens = kanaldata.Frekvens;
                var bandbredd1 = kanaldata.Bandbredd1;
                var bandbredd2 = kanaldata.Bandbredd2;
                frekvens = frekvens.PadRight(5, '0');
                bandbredd1 = bandbredd1.PadRight(4, '0');
                bandbredd2 = bandbredd2.PadRight(4, '0');
                var result = frekvens + bandbredd1 + bandbredd2;
                return result;
            }
        }

        public Ra180Mod Mod
        {
            get { return _mod; }
            private set
            {
                if (_mod == value) return;

                var mod = _mod;
                if (mod == Ra180Mod.Från)
                {
                    _taskFactory.StartNew(() =>
                    {
                        var interval = TimeSpan.FromSeconds(2);
                        Action wait = () => _taskFactory.Wait(interval);
                        SetDisplay("TEST");
                        wait();
                        SetDisplay("TEST OK");
                        wait();
                        SetDisplay("NOLLST");
                        wait();

                        if (_isStarted)
                            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));

                        SetDisplay(null);
                        _mod = value;
                    });
                }
                else
                    _mod = value;
            }
        }

        public int Volym { get { return _volym; } }
        public string Display { get; private set; }

        public event EventHandler DisplayChanged;
        public event EventHandler NowChanged;

        protected virtual void OnNowChanged()
        {
            var handler = NowChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnDisplayChanged()
        {
            var handler = DisplayChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void SendKeys(Ra180Knapp knapp)
        {
            HandleHardwareKeys(knapp);

            if (Mod == Ra180Mod.Från)
                return;

            if (_currentMenu == null)
            {
                var keymaps = new Dictionary<Ra180Knapp, Func<Ra180Menu>>
                {
                    {Ra180Knapp.Knapp1, () => new Ra180MenuTid(this)},
                    {Ra180Knapp.Knapp2, () => new Ra180MenuRda(this)},
                    {Ra180Knapp.Knapp4, () => new Ra180MenuKda(this)},
                    {Ra180Knapp.Knapp7, () => new Ra180MenuNyk(this)},
                };

                foreach (var keymap in keymaps)
                {
                    if (keymap.Key != knapp) continue;
                    _currentMenu = keymap.Value();
                }
            }
            else
            {
                _currentMenu.HandleKeys(knapp);
            }
        }

        internal DateTimeOffset Now { get; private set; }

        internal Ra180Kanaldata Kanaldata
        {
            get
            {
                switch (Kanal)
                {
                    case 1: return Hemligt.Kanal1;
                    case 2: return Hemligt.Kanal2;
                    case 3: return Hemligt.Kanal3;
                    case 4: return Hemligt.Kanal4;
                    case 5: return Hemligt.Kanal5;
                    case 6: return Hemligt.Kanal6;
                    case 7: return Hemligt.Kanal7;
                    case 8: return Hemligt.Kanal8;
                    default:
                        return new Ra180Kanaldata();
                }
            }
        }

        internal Ra180Hemligt Hemligt
        {
            get { return _hemligt ?? (_hemligt = Ra180Hemligt.GetDefault()); }
        }

        public event EventHandler<string> ReceivedString;

        protected virtual void OnReceivedString(string e)
        {
            EventHandler<string> handler = ReceivedString;
            if (handler != null) handler(this, e);
        }

        internal void SetDisplay(string text)
        {
            Display = text;
            OnDisplayChanged();
        }

        internal void SetNow(DateTimeOffset now)
        {
            Now = now;
            OnNowChanged();
        }

        private void HandleHardwareKeys(Ra180Knapp knapp)
        {
            var keymaps = new Dictionary<Ra180Knapp, Action>
            {
                {Ra180Knapp.ModFrån, () => Mod = Ra180Mod.Från},
                {Ra180Knapp.ModKlar, () => Mod = Ra180Mod.Klar},
                {Ra180Knapp.ModSkydd, () => Mod = Ra180Mod.Skydd},
                {Ra180Knapp.ModDRelä, () => Mod = Ra180Mod.DRelä},
                {Ra180Knapp.Volym1, () => _volym = 1},
                {Ra180Knapp.Volym2, () => _volym = 2},
                {Ra180Knapp.Volym3, () => _volym = 3},
                {Ra180Knapp.Volym4, () => _volym = 4},
                {Ra180Knapp.Volym5, () => _volym = 5},
                {Ra180Knapp.Volym6, () => _volym = 6},
                {Ra180Knapp.Volym7, () => _volym = 7},
                {Ra180Knapp.Volym8, () => _volym = 8},
                {Ra180Knapp.Kanal1, () => _kanal = 1},
                {Ra180Knapp.Kanal2, () => _kanal = 2},
                {Ra180Knapp.Kanal3, () => _kanal = 3},
                {Ra180Knapp.Kanal4, () => _kanal = 4},
                {Ra180Knapp.Kanal5, () => _kanal = 5},
                {Ra180Knapp.Kanal6, () => _kanal = 6},
                {Ra180Knapp.Kanal7, () => _kanal = 7},
                {Ra180Knapp.Kanal8, () => _kanal = 8},
            };

            foreach (var keymap in keymaps)
            {
                if (knapp.HasFlag(keymap.Key))
                    keymap.Value();
            }
        }

        internal void CloseMenu()
        {
            _currentMenu = null;
            SetDisplay(null);
        }

        public void Dispose()
        {
            _timer.Dispose();
            _transport.ReceivedString -= ReceiveString;
        }

        public void SendString(string s)
        {
            if (s == null) throw new ArgumentNullException("s");
            if (string.IsNullOrEmpty(s)) throw new ArgumentException();

            var messageHeader = GetMessageHeader();
            var message = messageHeader + '|' + s;
            _transport.SendString(message);
        }

        private string GetMessageHeader()
        {
            var header = new StringBuilder();
            header.Append(_id);
            header.Append(Kanaldata.Frekvens);
            
            if (Mod == Ra180Mod.Skydd)
            {
                header.Append(Kanaldata.Bandbredd1);
                header.Append(Kanaldata.Bandbredd2);
                header.Append(Kanaldata.NYK);
            }

            return header.ToString();
        }

        public void ReceiveString(string s)
        {
            if (s == null) throw new ArgumentNullException("s");

            if (Mod == Ra180Mod.Från)
                return;

            var headerPos = s.IndexOf('|');
            if (headerPos == -1) return;
            var header = s.Substring(0, headerPos);
            var message = s.Substring(headerPos + 1);

            var idLength = _id.ToString().Length;
            var remoteId = header.Substring(0, idLength);
            header = header.Substring(idLength);

            if (string.Equals(remoteId, _id.ToString(), StringComparison.OrdinalIgnoreCase))
                return;

            if (Mod == Ra180Mod.Klar)
            {
                if (header != Kanaldata.Frekvens) return;
            }
            else if (Mod == Ra180Mod.Skydd)
            {
                var hoppgruppnyckel = Kanaldata.Frekvens +
                    Kanaldata.Bandbredd1 +
                    Kanaldata.Bandbredd2 +
                    Kanaldata.NYK;
                var remoteHgny = header.Substring(0, hoppgruppnyckel.Length);
                header = header.Substring(hoppgruppnyckel.Length);
                if (remoteHgny != hoppgruppnyckel)
                    return;
            }

            OnReceivedString(message);
        }
    }
}
