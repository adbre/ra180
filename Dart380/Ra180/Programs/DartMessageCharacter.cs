using System;

namespace Ra180.Programs
{
    public class DartMessageCharacter
    {
        private readonly bool _isReadOnly;
        private char _c;

        public DartMessageCharacter(char c, bool isReadOnly)
        {
            _c = c;
            _isReadOnly = isReadOnly;
        }

        public DartMessageCharacter() : this('\0')
        {
        }

        public DartMessageCharacter(char c)
        {
            _c = c;
        }

        public char Char
        {
            get { return _c; }
            set
            {
                if (_isReadOnly) throw new InvalidOperationException("Object is read-only");
                _c = value;
            }
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
        }

        public DartMessageCharacter ReadOnly()
        {
            return new DartMessageCharacter(_c, true);
        }
    }
}