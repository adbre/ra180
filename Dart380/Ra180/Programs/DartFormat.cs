using System;
using System.Collections.Generic;
using System.Linq;

namespace Ra180.Programs
{
    public class DartFormat
    {
        private string[] _bodyFormat = new string[0];

        public DartFormat(int nr, string nivå1, string nivå2, string namn16tkn, string namn8tkn, params string[] body)
        {
            Nr = nr;
            Nivå1 = nivå1;
            Nivå2 = nivå2;
            Namn16Tecken = namn16tkn;
            Namn8Tecken = namn8tkn;
            BodyFormat = body ?? new [] {"TEXT:"};
        }

        public int Nr { get; private set; }
        public string Nivå1 { get; private set; }
        public string Nivå2 { get; private set; }
        public string Namn16Tecken { get; private set; }
        public string Namn8Tecken { get; private set; }

        public string[] BodyFormat
        {
            get { return _bodyFormat.ToArray(); }
            private set
            {
                if (value == null) throw new ArgumentNullException("value");
                var bodyFormat = new List<string>();
                for (var i=0; bodyFormat.Sum(str => str.ToCharArray().Count(c => c == ' ')) + 16 < 200; i++)
                {
                    var row = (i < value.Length ? value[i] : null) ?? "";
                    row = row.PadRight(16, ' ');
                    bodyFormat.Add(row);
                }

                _bodyFormat = bodyFormat.ToArray();
            }
        }
    }
}