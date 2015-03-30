using System;

namespace Ra180
{
    internal class Ra180EffProgram : Ra180MenuProgram
    {
        public Ra180EffProgram(Ra180 ra180, Ra180Display display)
            : base(ra180, display)
        {
            Title = "EFF";

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "EFF",
                CanEdit = () => true,
                OnKey = key =>
                {
                    if (key == Ra180Key.ÄND)
                    {
                        Ra180.Data.Eff = NextEff(Ra180.Data.Eff, HasRa480Connection());
                        return true;
                    }

                    return false;
                },
                GetValue = () =>
                {
                    var eff = Ra180.Data.Eff;
                    switch (eff)
                    {
                        case Ra180Eff.Låg: return "LÅG";
                        case Ra180Eff.Norm: return "NORM";
                        case Ra180Eff.Hög: return "HÖG";
                        default:
                            return null;
                    }
                }
            });
        }

        private Ra180Eff NextEff(Ra180Eff currentEff, bool hasRa480)
        {
            switch (currentEff)
            {
                case Ra180Eff.Låg: return Ra180Eff.Norm;
                case Ra180Eff.Norm:
                    return hasRa480 ? Ra180Eff.Hög : Ra180Eff.Låg;
                case Ra180Eff.Hög:
                    return Ra180Eff.Låg;
                default:
                    return Ra180Eff.Låg;
            }
        }

        private bool HasRa480Connection()
        {
            return false;
        }
    }
}