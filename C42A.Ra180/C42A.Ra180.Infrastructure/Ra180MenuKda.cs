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
        private bool _moveBackToSubmenu1;
        private string _pny;
        private int _pnyGrupp;

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
            if (Input != null) return;
            _moveBackToSubmenu1 = false;

            switch (Submenu)
            {
                case 0:
                case 1:
                case 2:
                    base.OnÄND();
                    break;

                case 4:
                    if (_pnyGrupp > 0) return;
                    _pnyGrupp = 1;
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
                case 4: return 4;
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

                    case 4:
                        _pny += input;
                        if (_pnyGrupp == 9)
                        {
                            Unit.Kanaldata.SetPNYGroups(_pny);
                            _pny = null;
                        }

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
                _moveBackToSubmenu1 = true;
            }
            else if (Submenu == 2)
            {
                if (_moveBackToSubmenu1)
                {
                    _moveBackToSubmenu1 = false;
                    Submenu = 1;
                }
            }
            else if (Submenu == 4)
            {
                if (_pnyGrupp == 9)
                {
                    _pnyGrupp = default(int);
                    OnInputChanged(null);
                    return;
                }

                _pnyGrupp++;
                StartCaptureInput();
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
                if (_pnyGrupp >= 1 && _pnyGrupp <= 9)
                    return string.Format("PN{0}:{1}", _pnyGrupp, input);

                var pny = Unit.Kanaldata.PNY;
                pny = pny ?? "###";
                return string.Format("PNY:{0}", pny);
            }

            return null;
        }
    }
}