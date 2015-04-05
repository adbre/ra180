using System;
using System.Linq;
using Ra180.Devices.Dart380;

namespace Ra180.Programs
{
    public class Dart380FmtProgram : ProgramBase<Dart380>
    {
        private Action _action;
        private string _key = "";
        private string _inputBuffer = "";
        private DartFormat _fmt;
        private DartMessageEditor _msgEditor;

        public Dart380FmtProgram(Dart380 device, Ra180Display display) : base(device, display)
        {
            LargeDisplay = new DisplayWriter(device.LargeDisplay);
            SmallDisplay = new DisplayWriter(device.SmallDisplay);

            Next(ReadFmt);
        }

        private DisplayWriter LargeDisplay { get; set; }
        private DisplayWriter SmallDisplay { get; set; }
        
        private void ReadFmt()
        {
            if (_key == Ra180Key.SLT)
            {
                Close();
                return;
            }

            if (_key == Ra180Key.ENT)
            {
                if (_inputBuffer.All(Char.IsDigit) && (_fmt = Device.Data.Format.GetFormat(_inputBuffer)) != null)
                {
                    Next(DisplayFmt);
                    return;
                }
            }

            if (_key.Length == 1)
                _inputBuffer += _key;

            LargeDisplay.SetText(string.Format("FORMAT:{0}", _inputBuffer));
            SmallDisplay.SetText(Device.Data.Format.GetShortName(_inputBuffer));
        }

        private void DisplayFmt()
        {
            if (_key == Ra180Key.SLT)
            {
                Close();
                return;
            }

            if (_key == Ra180Key.ENT)
            {
                var msg = new DartMessage(_fmt);
                msg.Sender = Device.Data.Address;
                msg.Timestamp = string.Format("{0:00}{1:00}{2:00}", Device.Ra180.Clock.Day, Device.Ra180.Clock.Hour, Device.Ra180.Clock.Minute);
                _msgEditor = new DartMessageEditor(msg);

                Next(ShowMsg);
                return;
            }

            LargeDisplay.SetText(_fmt.Namn16Tecken);
            SmallDisplay.SetText(_fmt.Namn8Tecken);
        }

        private void ShowMsg()
        {
            if (!NavigateMsg(_key))
            {
                switch (_key)
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
            if (!NavigateMsg(_key))
            {
                switch (_key)
                {
                    case Ra180Key.SLT:
                        Next(ShowMsg);
                        return;

                    default:
                        if (_key.Length == 1)
                            _msgEditor.Write(_key);
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
                case Dart380Key.PGDOWN:
                    _msgEditor.MoveDown();
                    return true;

                case Dart380Key.PGUP:
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

        private void Next(Action action)
        {
            _key = "";
            _action = action;
            action();
        }

        public override bool SendKey(string key)
        {
            _key = key;
            _action();
            _key = "";
            return true;
        }

        public override void UpdateDisplay()
        {
            if (IsClosed)
            {
                LargeDisplay.Clear();
                SmallDisplay.Clear();
                base.UpdateDisplay();
                return;
            }

            LargeDisplay.Refresh();
            SmallDisplay.Refresh();
        }
    }
}