using System.Collections.Generic;
using System.Linq;

namespace Ra180.Programs
{
    public class DartMessage
    {
        private readonly DartFormat _format;

        private readonly string[] _headerFormat =
        {
            "TILL:           ",
            "                ",
            "      *FR:      ",
            "                ",
            "FRÅN:     *U:   ",
        };

        private readonly string[] _bodyFormat;

        private readonly string[] _footerFormat =
        {
            "------SLUT------"
        };

        public DartMessage(DartFormat format)
        {
            _format = format;
            _bodyFormat = format.BodyFormat;
            Lines = Join(_headerFormat, _bodyFormat, _footerFormat).Select(formatLine => new DartMessageLine(formatLine)).ToArray();
        }

        public DartFormat Format
        {
            get { return _format; }
        }

        public DartMessageLine[] Lines { get; private set; }

        public string Timestamp
        {
            get { return Read(2, 0, 6); }
            set { Write(2, 0, 6, value); }
        }

        public string Sender
        {
            get { return Read(2, 10, 22); }
            set { Write(2, 10, 22, value); }
        }

        public string[] ToStringArray()
        {
            var lines = new string[Lines.Length];
            for (var i = 0; i < lines.Length; i++)
                lines[i] = Lines[i].ToString();

            return lines;
        }


        public DartMessage Clone()
        {
            var clone = new DartMessage(_format);

            for (var i = 0; i < Lines.Length; i++)
            {
                for (var j = 0; j < Lines[i].Characters.Length; j++)
                {
                    if (Lines[i].Characters[j].IsReadOnly) continue;
                    clone.Lines[i].Characters[j].Char = Lines[i].Characters[j].Char;
                }
            }

            return clone;
        }

        private string Read(int lineIndex, int index, int length)
        {
            return Lines[lineIndex].ToString().Substring(index, length);
        }

        private void Write(int lineIndex, int index, int length, string value)
        {
            Write(ref lineIndex, ref index, EnsureWidth(value, length));
        }

        internal void Write(ref int lineIndex, ref int index, string value)
        {
            var toBeWritten = new Queue<char>(value.ToCharArray());
            for (; lineIndex < Lines.Length && toBeWritten.Any(); lineIndex++)
            {
                for (; index < Lines[lineIndex].Characters.Length && toBeWritten.Any(); index++)
                {
                    if (Lines[lineIndex].Characters[index].IsReadOnly)
                        continue;

                    Lines[lineIndex].Characters[index].Char = toBeWritten.Dequeue();
                }

                if (!toBeWritten.Any())
                    break;
                
                index = 0;
            }
        }

        private static IEnumerable<string> Join(params IEnumerable<string>[] collections)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var collection in collections)
            {
                foreach (var item in collection)
                    yield return item;
            }
        }

        private static string EnsureWidth(string s, int width)
        {
            s = s ?? "";
            if (s.Length < width)
                s = s.PadRight(width);
            else if (s.Length > width)
                s = s.Substring(0, width);

            return s;
        }
    }
}