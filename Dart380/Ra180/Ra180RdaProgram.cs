namespace Ra180
{
    internal class Ra180RdaProgram : Ra180MenuProgram
    {
        public Ra180RdaProgram(Ra180 ra180, Ra180Display display) : this(ra180, display, false)
        {
        }

        public Ra180RdaProgram(Ra180 ra180, Ra180Display display, bool isDart380) : base(ra180, display)
        {
            Title = "RDA";

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "SDX",
                GetValue = () => "NEJ"
            });

            if (!isDart380)
            {
                AddChild(new Ra180EditMenuItem
                {
                    Prefix = () => "OPMTN",
                    GetValue = () => "JA"
                });
            }

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "BAT",
                GetValue = () => "13.1"
            });
        }
    }
}