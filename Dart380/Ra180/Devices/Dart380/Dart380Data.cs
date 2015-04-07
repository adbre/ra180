namespace Ra180.Devices.Dart380
{
    public class Dart380Data
    {
        private readonly DartFormatCollection _format = new DartFormatCollection();
        private readonly DartMessageStorage _messages = new DartMessageStorage();

        public Dart380Data()
        {
            Address = "*";
            Operat�rsmeddelandeton = true;
            Summer = true;
            Tangentklick = true;
        }

        public string Address { get; set; }
        public Dart380PrinterOption Printer { get; set; }
        public bool Operat�rsmeddelandeton { get; set; }
        public bool Summer { get; set; }
        public bool Tangentklick { get; set; }

        public DartMessageStorage Messages { get { return _messages; } }
        public DartFormatCollection Format { get { return _format; } }
    }
}