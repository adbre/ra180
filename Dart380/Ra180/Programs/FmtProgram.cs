using System;
using System.Linq;
using Ra180.Devices.Dart380;

namespace Ra180.Programs
{
    public class FmtProgram : Dart380Program
    {
        private string _inputBuffer = "";
        private DartFormat _fmt;
        private DartMessageEditor _msgEditor;

        public FmtProgram(Dart380 device) : base(device)
        {
        }

        protected override void Execute()
        {
            if (Key == Ra180Key.SLT)
            {
                Close();
                return;
            }

            if (Key == Ra180Key.ENT)
            {
                if (_inputBuffer.All(Char.IsDigit) && (_fmt = Device.Data.Format.GetFormat(_inputBuffer)) != null)
                {
                    Next(DisplayFmt);
                    return;
                }
            }

            if (Key.Length == 1)
                _inputBuffer += Key;

            LargeDisplay.SetText(string.Format("FORMAT:{0}", _inputBuffer));
            SmallDisplay.SetText(Device.Data.Format.GetShortName(_inputBuffer));
        }

        private void DisplayFmt()
        {
            if (Key == Ra180Key.SLT)
            {
                Close();
                return;
            }

            if (Key == Ra180Key.ENT)
            {
                var msg = new DartMessage(_fmt);
                msg.Sender = Device.Data.Address;
                msg.Timestamp = string.Format("{0:00}{1:00}{2:00}", Device.Ra180.Clock.Day, Device.Ra180.Clock.Hour, Device.Ra180.Clock.Minute);

                Device.Data.Messages.Isk.Add(msg);

                _msgEditor = new DartMessageEditor(msg);

                Next(ShowMsg);
                return;
            }

            LargeDisplay.SetText(_fmt.Namn16Tecken);
            SmallDisplay.SetText(_fmt.Namn8Tecken);
        }

        private void ShowMsg()
        {
            if (!NavigateMsg(Key))
            {
                switch (Key)
                {
                    case Ra180Key.SLT:
                        Close();
                        return;

                    case Ra180Key.ÄND:
                        Next(EditMsg);
                        return;
                }
            }

            LargeDisplay.SetText(_msgEditor.CurrentLine);
            SmallDisplay.SetText(_fmt.Namn8Tecken);
        }

        private void EditMsg()
        {
            if (!NavigateMsg(Key))
            {
                switch (Key)
                {
                    case Ra180Key.SLT:
                        Next(ShowMsg);
                        return;

                    default:
                        if (Key.Length == 1)
                            _msgEditor.Write(Key);
                        break;
                }
            }

            LargeDisplay.SetText(_msgEditor.CurrentLine);
            SmallDisplay.SetText(_fmt.Namn8Tecken);
        }

        private bool NavigateMsg(string key)
        {
            switch (key)
            {
                case Ra180Key.ENT:
                case Dart380Key.DARROW:
                    _msgEditor.MoveDown();
                    return true;

                case Dart380Key.UARROW:
                    _msgEditor.MoveUp();
                    return true;

                case Dart380Key.RARROW:
                    _msgEditor.MoveRight();
                    return true;

                case Dart380Key.LARROW:
                    _msgEditor.MoveLeft();
                    return true;
            }

            return false;
        }
    }
}