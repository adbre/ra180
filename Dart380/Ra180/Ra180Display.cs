using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ra180
{
	public class Ra180Display : ChangeableBase
	{
		private readonly List<Ra180DisplayCharacter> _characters;

	    public Ra180Display() : this(8)
	    {
	    }

		public Ra180Display (int length)
		{
			_characters = new List<Ra180DisplayCharacter>(length);
			for (var i = 0; i < length; i++) {
				var c = new Ra180DisplayCharacter ();
				c.Changed += OnCharacterChanged;
				_characters.Add (c); 
			}
		}

		public IReadOnlyCollection<Ra180DisplayCharacter> Characters {
			get { return _characters; }
		}

		public void SetText(string text) {
			SetText (text, new int[0], new int[0]);
		}

		public void SetText(string text, int? blinkingPosition = null, bool trailingUnderscore = false) {
			if (text == null) throw new ArgumentNullException ("text");
			var underscorePosition = trailingUnderscore ? text.Length : 0;
			SetText (text, blinkingPosition, underscorePosition);
		}

		public void SetText(string text, int? blinkingPosition = null, int? underscorePosition = null) {
			var blinkingPositions = blinkingPosition != null ? new [] { blinkingPosition.Value } : null;
			var underscorePositions = underscorePosition != null ? new [] { underscorePosition.Value } : null;
			SetText (text, blinkingPositions, underscorePositions);
		}

		public void SetText(string text, int[] blinkingPositions, int[] underscorePositions) {
			if (text == null) throw new ArgumentNullException ("text");
			if (text.Length > _characters.Count) throw new ArgumentException (string.Format ("text must not be longer than size of display ({0} characters)", _characters.Count), "text");

			this.Update (() => {
				for (var i=0; i < _characters.Count; i++) {
					_characters[i].Update(character => {
						character.Char = text != null && text.Length > i ? text[i] : ' ';
						character.IsBlinking = blinkingPositions != null && blinkingPositions.Contains(i);
						character.HasUnderscore = underscorePositions != null && underscorePositions.Contains(i);
					});
				}
			});
		}

		public void Clear() {
			SetText (string.Empty);
		}

		public override string ToString ()
		{
			var result = new StringBuilder (_characters.Count);
			foreach (var c in _characters) {
				result.Append (c.Char);
			}

			return result.ToString ();
		}

		protected virtual void OnCharacterChanged (object sender, EventArgs e)
		{
			if (IsSuspended) {
				SetHasPendingChange ();
			} else {
				OnChanged (e);
			}
		}
	}

}
