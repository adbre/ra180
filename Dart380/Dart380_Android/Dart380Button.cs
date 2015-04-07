namespace Dart380_Android
{
	public class Dart380Button : Dart380Control
	{
		public enum ButtonStyle
		{
			Normal,
			Toggle
		}

		public Dart380Button (string primaryKey, int x, int y, int w, int h)
			: this(primaryKey, null, x, y, w, h)
		{
		}

		public Dart380Button(string primaryKey, string secondaryKey, int x, int y, int w, int h)
			: base(x, y, w, h)
		{
			PrimaryKey = primaryKey;
			SecondaryKey = secondaryKey;
		}

		public string PrimaryKey { get; set; }
		public string SecondaryKey { get; set; }

		public bool IsPressed { get; set; }
		public ButtonStyle Style { get; set; }

		protected override void OnTouchEventDown()
		{
			base.OnTouchEventDown ();

			if (Style == ButtonStyle.Normal)
				IsPressed = true;
			else if (Style == ButtonStyle.Toggle)
				IsPressed = !IsPressed;
		}

		protected override void OnTouchEventUp()
		{
			base.OnTouchEventUp ();
			if (Style == ButtonStyle.Normal)
				IsPressed = false;
		}
	}
}
