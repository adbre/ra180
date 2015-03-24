using System;
using Android.Views;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Graphics;

namespace Dart380_Android
{

	public static class BitmapExtensions
	{
		public static Rect GetSize(this Bitmap bitmap) {
			return new Rect (0, 0, bitmap.Width, bitmap.Height);
		}
	}
}
