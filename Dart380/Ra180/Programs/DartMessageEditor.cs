namespace Ra180.Programs
{
    public class DartMessageEditor
    {
        private readonly DartMessage _message;
        private int _lineIndex = -1;
        private int _index;

        public DartMessageEditor(DartMessage message)
        {
            _message = message;
            MoveDown();
        }

        public DartMessage Message
        {
            get { return _message; }
        }

        public string CurrentLine
        {
            get { return _message.Lines[_lineIndex].ToString(); }
        }

        public string[] ToStringArray()
        {
            return _message.ToStringArray();
        }

        public void Erase()
        {
        }

        public void Write(char c)
        {
            _message.Write(ref _lineIndex, ref _index, c.ToString());
        }

        public void Write(string s)
        {
            foreach (var c in s.ToCharArray())
                Write(c);
        }

        public void MoveRight()
        {
            
        }

        public void MoveLeft()
        {
            
        }

        public void MoveUp()
        {
            _lineIndex--;
            if (_lineIndex < 0)
                _lineIndex = 0;
        }

        public void MoveDown()
        {
            _lineIndex++;
            if (_lineIndex > _message.Lines.Length)
                _lineIndex = _message.Lines.Length;

            _index = FindNextWriteableIndex(_message.Lines[_lineIndex].Characters);
            if (_index < 0)
                _index = 0;
        }

        private int FindNextWriteableIndex(DartMessageCharacter[] characters)
        {
            for (var i = 0; i < characters.Length; i++)
            {
                if (!characters[i].IsReadOnly)
                    return i;
            }

            return -1;
        }
    }
}