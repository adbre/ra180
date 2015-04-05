using System.Linq;
using System.Text;

namespace Ra180.Programs
{
    public class DartMessageLine
    {
        private readonly DartMessageCharacter[] _characters;

        public DartMessageLine(DartMessageCharacter[] characters)
        {
            _characters = characters;
        }

        public DartMessageLine() : this(16)
        {
        }

        public DartMessageLine(int length) : this(new DartMessageCharacter[length])
        {
        }

        public DartMessageLine(string text)
        {
            _characters = text.ToCharArray().Select(c => new DartMessageCharacter(c, c != ' ')).ToArray();
        }

        public DartMessageCharacter[] Characters
        {
            get { return _characters; }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var character in _characters)
                result.Append(character.Char);
            return result.ToString();
        }
    }
}