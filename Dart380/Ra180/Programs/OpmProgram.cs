using System.Collections.Generic;
using System.Linq;
using Ra180.Devices.Dart380;

namespace Ra180.Programs
{
    public class OpmProgram : Dart380Program
    {
        private Queue<Operatörsmeddelande> _messages;
        private Operatörsmeddelande _opm;

        public OpmProgram(Dart380 dart380) : base(dart380)
        {
        }

        protected override void Execute()
        {
            if (_messages == null)
                LoadMessages();
            
            if (Key == Dart380Key.SLT)
            {
                Close();
                return;
            }

            if (Key == Dart380Key.OPM)
            {
                if (_messages != null && _messages.Count > 0)
                {
                    ReadNextMessage();
                }
                else if (_opm != null)
                {
                    ReadLastMessage();
                }
                else
                {
                    Close();
                    return;
                }
            }

            LargeDisplay.SetText(string.Empty);

            if (_opm != null)
            {
                SmallDisplay.SetText(_opm.Text);
            }
            else
            {
                SmallDisplay.CenterText("(OPM)");
            }
        }

        private void ReadNextMessage()
        {
            _opm = _messages.Dequeue();
            Device.Data.Operatörsmeddelanden.Remove(_opm);
        }

        private void ReadLastMessage()
        {
            _opm = null;
        }

        private void LoadMessages()
        {
            _messages = new Queue<Operatörsmeddelande>(Device.Data.Operatörsmeddelanden);
            var ra180 = Device.Mik2 as Ra180;
            var isFtr = true;
            if (ra180 == null)
            {
                ra180 = Device.Tvåtråd as Ra180;
                isFtr = false;
            }

            if (ra180 != null)
            {
                if (new[] { Dart380Mod.KLAR, Dart380Mod.SKYDD, Dart380Mod.DRELÄ }.Contains(Device.Mod))
                    _messages.Enqueue(new Operatörsmeddelande { Text = isFtr ? "ST=FTR/F" : "ST=TTR/F" });
                else if (Device.Mod == Dart380Mod.TE)
                    _messages.Enqueue(new Operatörsmeddelande { Text = isFtr ? "ST=FTR" : "ST=TTR" });
            }

            ReadNextMessage();
        }
    }
}