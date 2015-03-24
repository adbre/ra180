using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ra180
{
    public class LedDisplay : IEnumerable<LedCharacter>
    {
        private const int BRIGHTNESS_MINIMUM = 0;
        private const int BRIGHTNESS_MAXIMUM = 5;
        private const int BRIGHTNESS_DEFAULT = 3;

        private readonly LedCharacter[] _characters;

        private bool _suppressPropertyChangedEvent;
        private bool _propertyChangedEventPending;
        private int _brightness = BRIGHTNESS_DEFAULT;

        public LedDisplay(int length)
        {
            _characters = new LedCharacter[length];
            for (var i = 0; i < length; i++)
            {
                _characters[i] = new LedCharacter();
                _characters[i].PropertyChanged += OnCharacterChanged;
            }
        }

        ~LedDisplay()
        {
            if (_characters != null)
            {
                foreach (var character in _characters)
                {
                    if (character != null)
                        character.PropertyChanged -= OnCharacterChanged;
                }
            }
        }

        public event EventHandler Changed;

        public int Count
        {
            get { return _characters.Length; }
        }

        public int Brightness { get { return _brightness; } }

        public LedCharacter this[int index]
        {
            get { return _characters[index]; }
        }

        public void ChangeBrightness()
        {
            _brightness++;

            if (_brightness > BRIGHTNESS_MAXIMUM)
                _brightness = BRIGHTNESS_MINIMUM;
        }

        public void SuspendLayout()
        {
            if (_suppressPropertyChangedEvent)
                return;

            _suppressPropertyChangedEvent = true;
            _propertyChangedEventPending = false;
        }

        public void ResumeLayout()
        {
            if (!_suppressPropertyChangedEvent)
                return;

            _suppressPropertyChangedEvent = false;

            if (_propertyChangedEventPending)
                OnCharacterChanged();

            _propertyChangedEventPending = false;
        }

        public void Clear()
        {
            SuspendLayout();

            foreach (var character in _characters)
                character.Clear();

            ResumeLayout();
        }

        public void SetTextFormat(string format, params object[] args)
        {
            var text = string.Format(format, args);
            SetText(text);
        }

        public void SetText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Clear();
                return;
            }

            if (text.Length > Count)
                throw new ArgumentException(string.Format("Given text is longer than what fits into the display. Maximum characters allows are {0}. Text was: {1}", Count, text));

            SuspendLayout();
            InnerSetText(text);
            ResumeLayout();
        }

        public override string ToString()
        {
            var result = new StringBuilder(Count);
            foreach (var ledCharacter in this)
            {
                result.Append(ledCharacter.Character);
            }
            return result.ToString();
        }

        private void InnerSetText(string text)
        {
            for (var i = 0; i < Count; i++)
            {
                var character = _characters[i];
                character.Clear();
                if (text.Length > i)
                    character.Character = text[i];
            }
        }

        public IEnumerator<LedCharacter> GetEnumerator()
        {
            return _characters.Cast<LedCharacter>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnCharacterChanged()
        {
            OnCharacterChanged(EventArgs.Empty);
        }

        private void OnCharacterChanged(EventArgs e)
        {
            if (_suppressPropertyChangedEvent)
            {
                _propertyChangedEventPending = true;
                return;
            }

            var handler = Changed;
            if (handler != null) handler(this, e);
        }

        private void OnCharacterChanged(object sender, EventArgs e)
        {
            OnCharacterChanged(e);
        }
    }
}