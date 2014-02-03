using System;
using System.Linq;

namespace C42A.Ra180.Infrastructure
{
    internal class Ra180Kanaldata
    {
        private bool _writeNyk1Last = false;

        public string Frekvens { get; set; }
        public string Bandbredd1 { get; set; }
        public string Bandbredd2 { get; set; }
        public string PNY { get; set; }
        public int ValdNyckel { get; set; }

        public string NYK
        {
            get { return GetNYK(ValdNyckel); }
        }

        public string GetNYK(int valdNyckel)
        {
            if (valdNyckel == 1)
                return NYK1;
            if (valdNyckel == 2)
                return NYK2;
            return null;
        }

        public string NYK1 { get; set; }
        public string NYK2 { get; set; }

        public void SetPNYGroups(string s)
        {
            if (s == null) throw new ArgumentNullException("s");
            if (!s.ToCharArray().All(c => Char.IsDigit(c)))
                throw new ArgumentException("s must only contain digits");

            const int expectedlength = 4*9;
            if (s.Length != expectedlength)
                throw new ArgumentException(string.Format("PNY group data must be exactly {0} digits long", expectedlength));

            var pny = s.Substring(0, 3);
            PNY = pny;

            var nyk = s + pny;

            if (!_writeNyk1Last)
            {
                NYK1 = nyk;
                _writeNyk1Last = true;
            }
            else
            {
                NYK2 = nyk;
                _writeNyk1Last = false;
            }
        }
    }
}