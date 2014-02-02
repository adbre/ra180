    using System;
    using System.Linq;

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
            if (Submenu != 0)
                return;

            base.OnÄND();
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
                case 0: return 5;
                case 1: return 4;
                case 2: return 4;
                default: return 0;
            }
        }

        protected override void OnInputChanged(string input)
        {
            if (Submenu != 0) return;
            var text = string.Format("FR:{0}", input);
            Unit.SetDisplay(text);
        }

        protected override bool TrySubmitInput(string input)
        {
            if (Submenu == 0)
                return TrySubmitFrekvensInput(input);
            return base.TrySubmitInput(input);
        }

        private bool TrySubmitFrekvensInput(string input)
        {
            var allDigits = input.ToCharArray().All(c => Char.IsDigit(c));
            if (!allDigits) return false;
            Unit.Kanaldata.Frekvens = input;
            return true;
        }

        protected override string FormatDisplay(int submenu)
        {
            if (submenu == 0)
                return string.Format("FR:{0}", Unit.Kanaldata.Frekvens);
            if (submenu == 1)
                return string.Format("BD1:{0}", Unit.Kanaldata.Bandbredd1);
            if (submenu == 2)
                return string.Format("BD2:{0}", Unit.Kanaldata.Bandbredd2);
            if (submenu == 3)
                return string.Format("SYNK=NEJ");
            if (submenu == 4)
            {
                var pny = Unit.Kanaldata.PNY;
                pny = pny ?? "###";
                return string.Format("PNY:{0}", pny);
            }

            return base.FormatDisplay(submenu);
        }
    }
}