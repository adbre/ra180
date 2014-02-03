namespace C42A.Ra180.Infrastructure
{
    internal class Ra180MenuNyk : Ra180Menu
    {
        private int _valdNyckel;

        public Ra180MenuNyk(Ra180Unit unit) : base(unit)
        {
        }

        protected override string Title
        {
            get { return "NYK"; }
        }

        protected override int Submenus
        {
            get { return 1; }
        }

        protected override void OnSLT()
        {
            _valdNyckel = default(int);
            base.OnSLT();
        }

        protected override void OnÄND()
        {
            if (Submenu == 0)
            {
                if (_valdNyckel == 0)
                {
                    if (Unit.Kanaldata.NYK1 == null && Unit.Kanaldata.NYK2 == null)
                        return;

                    _valdNyckel = Unit.Kanaldata.ValdNyckel;
                }

                if (_valdNyckel == 1)
                    _valdNyckel = 2;
                else
                    _valdNyckel = 1;

                var text = FormatDisplay(Submenu);
                Unit.SetDisplay(text);
                return;
            }

            base.OnÄND();
        }

        protected override void OnRETURN()
        {
            if (Submenu == 0 && _valdNyckel > 0)
            {
                Unit.Kanaldata.ValdNyckel = _valdNyckel;
                _valdNyckel = default(int);
                return;
            }

            base.OnRETURN();
        }

        protected override string FormatDisplay(int submenu)
        {
            if (submenu == 0)
            {
                var valdNyckel = _valdNyckel;
                if (valdNyckel == default (int))
                    valdNyckel = Unit.Kanaldata.ValdNyckel;
                var nyk = Unit.Kanaldata.GetNYK(valdNyckel) ?? "###";
                nyk = nyk.Substring(nyk.Length - 3);
                return string.Format("NYK={0}", nyk);
            }

            return base.FormatDisplay(submenu);
        }
    }
}