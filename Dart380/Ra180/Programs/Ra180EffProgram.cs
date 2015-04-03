using System;

namespace Ra180.Programs
{
    internal class Ra180EffProgram : Ra180MenuProgram
    {
        public Ra180EffProgram(Ra180 ra180, Ra180Display display)
            : base(ra180, display)
        {
            AddChild(new Ra180ComboBoxEditMenuItem("L�G", "NORM", "H�G")
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
                case Ra180Eff.L�g: return Ra180Eff.Norm;
                case Ra180Eff.Norm:
                    return hasRa480 ? Ra180Eff.H�g : Ra180Eff.L�g;
                case Ra180Eff.H�g:
                    return Ra180Eff.L�g;
                default:
                    return Ra180Eff.L�g;
            }
        }

        private bool HasRa480Connection()
        {
            return false;
        }
    }
}