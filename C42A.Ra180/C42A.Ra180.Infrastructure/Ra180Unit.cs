using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace C42A.Ra180.Infrastructure
{
    public interface ITaskFactory
    {
        void StartNew(Action action);
        void Wait(TimeSpan delay);
    }

    public class TaskFactory : ITaskFactory
    {
        public void StartNew(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            Task.Factory.StartNew(action);
        }

        public void Wait(TimeSpan delay)
        {
            Thread.Sleep(delay);
        }
    }

    public class DummyTaskFactory : ITaskFactory
    {
        public void StartNew(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            action();
        }

        public void Wait(TimeSpan delay)
        {
            
        }
    }

    public class Ra180Unit
    {
        private readonly ITaskFactory _taskFactory;
        private int _kanal = 1;
        private int _volym = 4;
        private Ra180Menu _currentMenu;
        private Ra180Hemligt _hemligt;
        private Ra180Mod _mod;

        public Ra180Unit(ITaskFactory taskFactory)
        {
            if (taskFactory == null) throw new ArgumentNullException("taskFactory");
            _taskFactory = taskFactory;
        }

        public int Kanal { get { return _kanal; }}

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
                        SetDisplay("NOLLSTÄ");
                        wait();
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

        public Ra180Unit()
        {
            Now = new DateTimeOffset(2016, 01, 01, 00, 00, 00, TimeSpan.Zero);
        }

        public event EventHandler DisplayChanged;

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

        internal void SetDisplay(string text)
        {
            Display = text;
            OnDisplayChanged();
        }

        internal void SetNow(DateTimeOffset now)
        {
            Now = now;
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
    }

    internal class Ra180Hemligt
    {
        public Ra180Hemligt()
        {
            Kanal1 = new Ra180Kanaldata();
            Kanal2 = new Ra180Kanaldata();
            Kanal3 = new Ra180Kanaldata();
            Kanal4 = new Ra180Kanaldata();
            Kanal5 = new Ra180Kanaldata();
            Kanal6 = new Ra180Kanaldata();
            Kanal7 = new Ra180Kanaldata();
            Kanal8 = new Ra180Kanaldata();
        }

        public Ra180Kanaldata Kanal1 { get; set; }
        public Ra180Kanaldata Kanal2 { get; set; }
        public Ra180Kanaldata Kanal3 { get; set; }
        public Ra180Kanaldata Kanal4 { get; set; }
        public Ra180Kanaldata Kanal5 { get; set; }
        public Ra180Kanaldata Kanal6 { get; set; }
        public Ra180Kanaldata Kanal7 { get; set; }
        public Ra180Kanaldata Kanal8 { get; set; }

        public static Ra180Hemligt GetDefault()
        {
            var result = new Ra180Hemligt();
            result.Kanal1.Frekvens = "30060";
            result.Kanal1.Bandbredd1 = "1234";
            result.Kanal1.Bandbredd2 = "5678";

            return result;
        }
    }

    internal class Ra180Kanaldata
    {
        public string Frekvens { get; set; }
        public string Bandbredd1 { get; set; }
        public string Bandbredd2 { get; set; }
        public string PNY { get; set; }
        public string NYK { get; set; }
    }
}
