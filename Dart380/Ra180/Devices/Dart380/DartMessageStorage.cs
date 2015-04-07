using System.Collections.Generic;
using Ra180.Programs;

namespace Ra180.Devices.Dart380
{
    public class DartMessageStorage
    {
        private readonly List<DartMessage> _mot = new List<DartMessage>();
        private readonly List<DartMessage> _avs = new List<DartMessage>();
        private readonly List<DartMessage> _isk = new List<DartMessage>();
        private readonly List<DartMessage> _ekv = new List<DartMessage>();

        public List<DartMessage> Mot { get { return _mot; } }
        public List<DartMessage> Ekv { get { return _ekv; } }
        public List<DartMessage> Avs { get { return _avs; } }
        public List<DartMessage> Isk { get { return _isk; } }
    }
}