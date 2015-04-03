using System;

namespace Ra180.Programs
{
    internal class Ra180EffProgram : Ra180MenuProgram
    {
        public Ra180EffProgram(Ra180 ra180, Ra180Display display)
            : base(ra180, display)
        {
            AddChild(new Ra180ComboBoxEditMenuItem("LÅG", "NORM", "HÖG")
            {
                Prefix = () => "EFF",

                OnSelectedIndex = index =>
                {
                    Device.Data.Eff = (Ra180Eff) Enum.ToObject(typeof (Ra180Eff), index);
                },

                GetSelectedIndex = () => (int)Device.Data.Eff
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