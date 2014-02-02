using System;
using System.Text.RegularExpressions;

namespace C42A.Ra180.Infrastructure
{
    internal class Ra180MenuTid : Ra180Menu
    {
        public Ra180MenuTid(Ra180Unit unit) : base(unit)
        {
            unit.NowChanged += UnitOnNowChanged;
        }

        protected override void CloseMenu()
        {
            Unit.NowChanged -= UnitOnNowChanged;
            base.CloseMenu();
        }

        private void UnitOnNowChanged(object sender, EventArgs eventArgs)
        {
            OnSubmenuChanged(Submenu);
        }

        protected override string Title
        {
            get { return "TID"; }
        }

        protected override int Submenus
        {
            get { return 2; }
        }

        protected override void OnNumpadKey(char key)
        {
            if (Input.Length >= GetMaxLength()) return;
            base.OnNumpadKey(key);
        }

        private int GetMaxLength()
        {
            switch (Submenu)
            {
                case 0: return 6;
                case 1: return 4;
                default: return 0;
            }
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