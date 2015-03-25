using System;
using System.Collections.Generic;

namespace Ra180
{
	public class Ra180DisplayCharacter : ChangeableBase
	{
		private char _char = ' ';
		private bool _isBlinking;
		private bool _hasUnderscore;

		public char Char {
			get { return _char; }
			set {
				var c = ReplaceControlChar (value);
				if (_char == c)
					return;

				_char = value;
				OnChanged (EventArgs.Empty);
			}
		}

		public bool IsBlinking {
			get { return _isBlinking; }
			set {
				if (_isBlinking == value)
					return;

				_isBlinking = value;
				OnChanged (EventArgs.Empty);
			}
		}

		public bool HasUnderscore {
			get { return _hasUnderscore; }
			set {
				if (_hasUnderscore == value)
					return;

				_hasUnderscore = value;
				OnChanged (EventArgs.Empty);
			}
		}

		private static char ReplaceControlChar(Char value) {
			if (Char.IsControl (value))
				return ' ';

			return value;
		}
	}
}
