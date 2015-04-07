using Android.Graphics;

namespace Dart380_Android
{
	public class Dart380Control : HotArea
	{
		public Dart380Control (int x, int y, int w, int h)
		{
			OriginalRect = new Rect (x, y, x + w, y + h);
		}
	}
}
