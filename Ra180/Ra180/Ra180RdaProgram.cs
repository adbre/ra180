namespace Ra180
{
    internal class Ra180RdaProgram : Ra180MenuProgram
    {
        public Ra180RdaProgram(Ra180 ra180) : base(ra180)
        {
            Title = "RDA";

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "SDX",
                GetValue = () => "NEJ"
            });

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "OPMTN",
                GetValue = () => "JA"
            });

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "BAT",
                GetValue = () => "13.1"
            });
        }
    }
}