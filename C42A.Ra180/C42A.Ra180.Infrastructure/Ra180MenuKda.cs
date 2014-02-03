    using System;
    using System.Linq;
    using Microsoft.SqlServer.Server;

namespace C42A.Ra180.Infrastructure
{
    /*
     * FR:
     * BD1:
     * BD2:
     * PNY:
     */

    internal class Ra180MenuKda : Ra180Menu
    {
        public Ra180MenuKda(Ra180Unit unit) : base(unit)
        {
        }

        protected override string Title
        {
            get { return "KDA"; }
        }

        protected override int Submenus
        {
            get { return 5; }
        }

        protected override void OnÄND()
        {
            switch (Submenu)
            {
                case 0:
                case 1:
                case 2:
                    base.OnÄND();
                    break;
            }
        }

        protected override void OnNumpadKey(char key)
        {
            var input = Input;
            if (input == null) return;
            if (input.Length >= GetMaxLength()) return;
            base.OnNumpadKey(key);
        }

        private int GetMaxLength()
        {
            switch (Submenu)
            {
                case 0: return 5;
                case 1: return 4;
                case 2: return 4;
                default: return 0;
            }
        }

        protected override void OnInputChanged(string input)
        {
            var text = FormatDisplay2(Submenu, input);
            Unit.SetDisplay(text);
        }

        protected override bool TrySubmitInput(string input)
        {
            if (IsAllDigits(input))
            {
                switch (Submenu)
                {
                    case 0:
                        Unit.Kanaldata.Frekvens = input;
                        return true;
                    case 1:
                        Unit.Kanaldata.Bandbredd1 = input;
                        return true;
                    case 2:
                        Unit.Kanaldata.Bandbredd2 = input;
                        return true;
                }
            }

            return base.TrySubmitInput(input);
        }

        protected override void ConfirmInput()
        {
            base.ConfirmInput();

            if (Submenu == 1)
            {
                NextSubmodule();
                OnÄND();
            }
        }

        private static bool IsAllDigits(string input)
        {
            return input.ToCharArray().All(c => Char.IsDigit(c));
        }

        protected override string FormatDisplay(int submenu)
        {
            var text = FormatDisplay2(submenu);
            text = text ?? base.FormatDisplay(submenu);
            return text;
        }

        private string FormatDisplay2(int submenu, string input = null)
        {
            if (submenu == 0)
                return string.Format("FR:{0}", input ?? Unit.Kanaldata.Frekvens);
            if (submenu == 1)
                return string.Format("BD1:{0}", input?? Unit.Kanaldata.Bandbredd1);
            if (submenu == 2)
                return string.Format("BD2:{0}", input ?? Unit.Kanaldata.Bandbredd2);
            if (submenu == 3)
                return string.Format("SYNK=NEJ");
            if (submenu == 4)
            {
                var pny = Unit.Kanaldata.PNY;
                pny = pny ?? "###";
                return string.Format("PNY:{0}", pny);
            }

            return null;
        }
    }
}