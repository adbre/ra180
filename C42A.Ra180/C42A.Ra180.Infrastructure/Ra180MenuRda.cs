namespace C42A.Ra180.Infrastructure
{
    internal class Ra180MenuRda : Ra180Menu
    {
        public Ra180MenuRda(Ra180Unit unit) : base(unit)
        {
        }

        protected override void OnÄND()
        {
            // Disabled
        }

        protected override string Title
        {
            get { return "RDA"; }
        }

        protected override int Submenus
        {
            get { return 3; }
        }

        protected override string FormatDisplay(int submenu)
        {
            if (submenu >= Submenus) return base.FormatDisplay(submenu);
            var menus = new[] { "SDX=NEJ", "OPMTN=JA", "BAT:12.5" };
            return menus[submenu];
        }
    }
}