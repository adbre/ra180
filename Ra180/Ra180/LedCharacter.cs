using System;

namespace Ra180
{
    public class LedCharacter
    {
        private const char EmptyCharacter = ' ';

        private bool _isFlashing;
        private bool _hasUnderscore;
        private char _character = EmptyCharacter;

        private bool _suppressPropertyChangedEvent;
        private bool _propertyChangedEventPending;

        public event EventHandler PropertyChanged;

        public bool IsFlashing
        {
            get { return _isFlashing; }
            set
            {
                if (_isFlashing == value)
                    return;

                _isFlashing = value;
                OnPropertyChanged();
            }
        }

        public bool HasUnderscore
        {
            get { return _hasUnderscore; }
            set
            {
                if (_hasUnderscore == value)
                    return;

                _hasUnderscore = value;
                OnPropertyChanged();
            }
        }

        public char Character
        {
            get { return _character; }
            set
            {
                if (_character == value)
                    return;

                _character = value;
                OnPropertyChanged();
            }
        }

        public void Clear()
        {
            SuspendLayout();
            IsFlashing = false;
            HasUnderscore = false;
            Character = EmptyCharacter;
            ResumeLayout();
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
                OnPropertyChanged();

            _propertyChangedEventPending = false;
        }

        protected virtual void OnPropertyChanged()
        {
            OnPropertyChanged(EventArgs.Empty);
        }

        protected virtual void OnPropertyChanged(EventArgs e)
        {
            if (_suppressPropertyChangedEvent)
            {
                _propertyChangedEventPending = true;
                return;
            }

            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
    }
}