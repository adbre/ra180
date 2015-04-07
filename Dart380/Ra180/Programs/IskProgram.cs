using System.Collections.Generic;
using System.Linq;
using Ra180.Devices.Dart380;

namespace Ra180.Programs
{
    public class IskProgram : Dart380Program
    {
        private int _messageIndex = 0;
        private DartMessageEditor _editor;
        private DartMessage _messagePendingDelete;
                
        public IskProgram(Dart380 device) : base(device)
        {
        }

        protected List<DartMessage> Messages
        {
            get { return Device.Data.Messages.Isk; }
        }

        protected DartMessage CurrentMessage
        {
            get { return Messages.Skip(_messageIndex).FirstOrDefault(); }
        }

        protected override void Execute()
        {
            var currentMessageIndex = _messageIndex;

            switch (Key)
            {
                case Ra180Key.SLT:
                    Close();
                    return;

                case Ra180Key.ENT:
                case Dart380Key.PGDOWN:
                    _messageIndex++;
                    if (_messageIndex > Messages.Count)
                        _messageIndex = Messages.Count;
                    break;

                case Dart380Key.PGUP:
                    _messageIndex--;
                    if (_messageIndex < 0)
                        _messageIndex = 0;
                    break;

                case Dart380Key.UARROW:
                    if (_editor != null)
                        _editor.MoveUp();
                    break;

                case Dart380Key.DARROW:
                    if (_editor != null)
                        _editor.MoveDown();
                    break;

                case Dart380Key.LARROW:
                    if (_editor != null)
                        _editor.MoveLeft();
                    break;

                case Dart380Key.RARROW:
                    if (_editor != null)
                        _editor.MoveRight();
                    break;
            }

            var currentMessage = CurrentMessage;
            if (currentMessage == null)
            {
                LargeDisplay.CenterText("(INSKRIVNA)");
                SmallDisplay.Clear();
                return;
            }

            switch (Key)
            {
                case Dart380Key.SND:
                    currentMessage = currentMessage.Clone();
                    LargeDisplay.CenterText("SÄNDER");
                    Device.Ra180.Radio.SendDataMessage(currentMessage.ToStringArray(), () =>
                    {
                        Device.Data.Messages.Avs.Add(currentMessage);
                        LargeDisplay.CenterText("SÄNT");
                    });
                    return;

                case Dart380Key.RAD:
                    _messagePendingDelete = currentMessage;
                    Next(ConfirmDelete);
                    return;
            }

            if (_editor == null || currentMessageIndex != _messageIndex)
            {
                _editor = new DartMessageEditor(currentMessage);
                _editor.MoveDown();
                _editor.MoveDown();
            }

            LargeDisplay.SetText(_editor.CurrentLine);
            SmallDisplay.SetText(currentMessage.Format.Namn8Tecken);
        }

        private void ConfirmDelete()
        {
            if (string.IsNullOrEmpty(Key))
            {
                LargeDisplay.CenterText("RADERA?");
                return;
            }

            if (Key == Dart380Key.RAD)
            {
                if (_messagePendingDelete != null)
                    Messages.Remove(_messagePendingDelete);

                LargeDisplay.CenterText("RADERAD");
                return;
            }

            Next(Execute);
        }
    }
}