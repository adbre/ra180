using System;
using System.Linq;

namespace Ra180.Programs
{
    internal class Ra180TidProgram : Ra180MenuProgram
    {
        public Ra180TidProgram(Ra180 ra180, Ra180Display display) : base(ra180, display)
        {
            Title = "TID";

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "T",
                MaxInputTextLength = () => 6,
                CanEdit = () => true,
                AcceptInput = (text, key) => key.All(Char.IsDigit),
                SaveInput = text =>
                {
                    if (text.Length == 6 && ra180.Clock.TrySetTime(text))
                        return true;

                    return false;
                },
                GetValue = () => ra180.Clock.GetTime()
            });

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "DAT",
                MaxInputTextLength = () => 4,
                CanEdit = () => true,
                AcceptInput = (text, key) => key.All(Char.IsDigit),
                SaveInput = text =>
                {
                    if (text.Length == 4 && ra180.Clock.TrySetDate(text))
                        return true;

                    return false;
                },
                GetValue = () => ra180.Clock.GetDate()
            });
        }
    }
}