using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

namespace C42A.Ra180.Infrastructure
{
    public class Ra180Unit
    {
        private int _kanal = 1;
        private int _volym = 4;
        private Ra180Menu _currentMenu;

        public int Kanal { get { return _kanal; }}
        public Ra180Mod Mod { get; private set; }
        public int Volym { get { return _volym; } }
        public string Display { get; private set; }

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

        internal void SetDisplay(string text)
        {
            Display = text;
        }

        internal void SetNow(DateTimeOffset now)
        {
            Now = now;
        }

        private void HandleHardwareKeys(Ra180Knapp knapp)
        {
            var keymaps = new Dictionary<Ra180Knapp, Action>
            {
                {Ra180Knapp.Från, () => Mod = Ra180Mod.Från},
                {Ra180Knapp.Klar, () => Mod = Ra180Mod.Klar},
                {Ra180Knapp.Skydd, () => Mod = Ra180Mod.Skydd},
                {Ra180Knapp.DRelä, () => Mod = Ra180Mod.DRelä},
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

    internal abstract class Ra180Menu
    {
        private readonly Ra180Unit _unit;
        private string _input;
        private int _submenu;

        protected Ra180Menu(Ra180Unit unit)
        {
            if (unit == null) throw new ArgumentNullException("unit");
            _unit = unit;

            OnSubmenuChanged(0);
        }

        protected virtual string Title
        {
            get { return "????????"; }
        }

        protected virtual int Submenus
        {
            get { return 0; }
        }

        protected Ra180Unit Unit { get { return _unit; } }

        protected int Submenu
        {
            get { return _submenu; }
            set
            {
                _submenu = value > Submenus ? 0 : value;
                OnSubmenuChanged(_submenu);
            }
        }

        protected string Input
        {
            get { return _input; }
            private set
            {
                _input = value; 
                OnInputChanged(_input);
            }
        }

        protected virtual void OnInputChanged(string input)
        {
        }

        public virtual void HandleKeys(Ra180Knapp knapp)
        {
            switch (knapp)
            {
                case Ra180Knapp.ÄND:
                    OnÄND();
                    break;
                case Ra180Knapp.RETUR:
                    OnRETURN();
                    break;
                case Ra180Knapp.SLT:
                    OnSLT();
                    break;
                default:
                    OnNumpadKey(knapp);
                    break;
            }
        }

        protected virtual void OnÄND()
        {
            StartCaptureInput();
        }

        protected virtual void OnRETURN()
        {
            var input = Input;
            if (input != null)
                ConfirmInput();
            else
                NextSubmodule();
        }

        protected virtual void OnSLT()
        {
            var input = Input;
            if (input == null)
            {
                CloseMenu();
                return;
            }

            Input = null;
            OnSubmenuChanged(Submenu);
        }

        protected virtual void NextSubmodule()
        {
            var submodule = Submenu + 1;
            if (submodule > Submenus)
            {
                CloseMenu();
                return;
            }

            Submenu++;
        }

        protected virtual void CloseMenu()
        {
            Unit.CloseMenu();
        }

        protected virtual void ConfirmInput()
        {
            var success = TrySubmitInput(Input);
            if (success) 
                _input = null;
            else
                Input = "";
        }

        protected virtual bool TrySubmitInput(string input)
        {
            return true;
        }

        protected virtual void StartCaptureInput()
        {
            Input = "";
        }

        protected virtual void OnNumpadKey(Ra180Knapp knapp)
        {
            var numpad = new Dictionary<Ra180Knapp, char>
            {
                {Ra180Knapp.Knapp0, '0'},
                {Ra180Knapp.Knapp1, '1'},
                {Ra180Knapp.Knapp2, '2'},
                {Ra180Knapp.Knapp3, '3'},
                {Ra180Knapp.Knapp4, '4'},
                {Ra180Knapp.Knapp5, '5'},
                {Ra180Knapp.Knapp6, '6'},
                {Ra180Knapp.Knapp7, '7'},
                {Ra180Knapp.Knapp8, '8'},
                {Ra180Knapp.Knapp9, '9'},
            };

            foreach (var keymap in numpad)
            {
                if (keymap.Key != knapp) continue;
                OnNumpadKey(keymap.Value);
            }
        }

        protected virtual void OnNumpadKey(char key)
        {
            var input = Input;
            if (input == null) return;
            input += key;
            Input = input;
        }

        protected virtual void OnSubmenuChanged(int submenu)
        {
            var displayText = FormatDisplay(submenu);
            Unit.SetDisplay(displayText);
        }

        protected virtual string FormatDisplay(int submenu)
        {
            if (submenu == Submenus)
                return string.Format("  ({0}) ", Title);
            else
                return null;
        }
    }

    internal class Ra180MenuTid : Ra180Menu
    {
        public Ra180MenuTid(Ra180Unit unit) : base(unit)
        {
        }

        protected override string Title
        {
            get { return "TID"; }
        }

        protected override int Submenus
        {
            get { return 2; }
        }

        protected virtual string GetSubmenuPrefix(int submenu)
        {
            var prefixes = new[] {"T:", "DAT:"};
            if (submenu >= 0 && submenu < prefixes.Length)
                return prefixes[submenu];
            else
                return null;
        }

        protected override string FormatDisplay(int submenu)
        {
            var prefix = GetSubmenuPrefix(submenu);

            if (submenu == 0)
                return string.Format("{0}{1:HHmmss}", prefix, Unit.Now);
            else if (submenu == 1)
                return string.Format("{0}{1:MMdd}", prefix, Unit.Now);
            else
                return base.FormatDisplay(submenu);
        }

        protected override void OnInputChanged(string input)
        {
            var prefix = GetSubmenuPrefix(Submenu);
            Unit.SetDisplay(prefix + input);
        }

        protected override bool TrySubmitInput(string input)
        {
            var submenu = Submenu;
            if (submenu == 0)
                return TrySubmitTidInput(input);
            else if (submenu == 1)
                return TrySubmitDatumInput(input);
            return false;
        }

        private bool TrySubmitDatumInput(string input)
        {
            var match = Regex.Match(input, "^([0-9]{2})([0-9]{2})$");
            if (!match.Success) return false;

            var mm = int.Parse(match.Groups[1].Value);
            var dd = int.Parse(match.Groups[2].Value);

            try
            {
                const int leapYear = 2016;
                var date = new DateTime(leapYear, mm, dd, 0, 0, 0, DateTimeKind.Local);
                date = date.Add(Unit.Now.TimeOfDay);
                var now = new DateTimeOffset(date);
                Unit.SetNow(now);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TrySubmitTidInput(string input)
        {
            var match = Regex.Match(input, "^([0-9]{2})([0-9]{2})([0-9]{2})$");
            if (!match.Success) return false;

            var hh = int.Parse(match.Groups[1].Value);
            var mm = int.Parse(match.Groups[2].Value);
            var ss = int.Parse(match.Groups[3].Value);

            try
            {
                var timespan = new TimeSpan(hh, mm, ss);
                var now = Unit.Now.Date.Add(timespan);
                Unit.SetNow(now);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
