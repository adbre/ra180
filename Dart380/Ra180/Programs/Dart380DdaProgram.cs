using Ra180.Devices.Dart380;

namespace Ra180.Programs
{
    public class Dart380DdaProgram : MenuProgram<Dart380>
    {
        public Dart380DdaProgram(Dart380 dart380, Ra180Display display) : base(dart380, display)
        {
            Title = "DDA";

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "AD",
                CanEdit = () => true,
                SaveInput = value =>
                {
                    if (string.IsNullOrEmpty(value)) return false;
                    Device.DartData.Address = value;
                    return true;
                },
                GetValue = () => Device.DartData.Address
            });

            AddChild(new Ra180ComboBoxEditMenuItem("MAN","MOT","ALLA","AVS")
            {
                Prefix = () => "SKR",
                OnSelectedValue = value =>
                {
                    if (value == "ALLA")
                        Device.DartData.Printer = Dart380PrinterOption.All;
                    else if (value == "MOT")
                        Device.DartData.Printer = Dart380PrinterOption.Received;
                    else if (value == "AVS")
                        Device.DartData.Printer = Dart380PrinterOption.Transmitted;
                    else
                        Device.DartData.Printer = Dart380PrinterOption.Manual;
                },
                GetSelectedValue = () =>
                {
                    if (Device.DartData.Printer.HasFlag(Dart380PrinterOption.All))
                        return "ALLA";
                    if (Device.DartData.Printer.HasFlag(Dart380PrinterOption.Received))
                        return "MOT";
                    if (Device.DartData.Printer.HasFlag(Dart380PrinterOption.Transmitted))
                        return "AVS";

                    return "MAN";
                }
            });

            AddChild(new Ra180ComboBoxEditMenuItem("AV", "PÅ")
            {
                Prefix = () => "OPMTN",
                OnSelectedValue = value =>
                {
                    Device.DartData.Operatörsmeddelandeton = value != "AV";
                },
                GetSelectedValue = () => Device.DartData.Operatörsmeddelandeton ? "PÅ" : "AV"
            });

            AddChild(new Ra180ComboBoxEditMenuItem("FRÅN", "TILL")
            {
                Prefix = () => "SUM",
                OnSelectedValue = value =>
                {
                    Device.DartData.Summer = value != "FRÅN";
                },
                GetSelectedValue = () => Device.DartData.Summer ? "TILL" : "FRÅN"
            });

            AddChild(new Ra180ComboBoxEditMenuItem("FRÅN", "TILL")
            {
                Prefix = () => "TKL",
                OnSelectedValue = value =>
                {
                    Device.DartData.Tangentklick = value != "FRÅN";
                },
                GetSelectedValue = () => Device.DartData.Tangentklick ? "TILL" : "FRÅN"
            });

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "BAT",
                GetValue = () => "13.1"
            });
        }
    }
}