using System;
using Android.Views;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Graphics;

namespace Dart380_Android
{

	public static class DrawableExtensions
	{
		public static void SetBounds(this Drawable drawable, Rect rect)
		{
			if (drawable == null) throw new ArgumentNullException ("drawable");
			if (rect == null) throw new ArgumentNullException ("rect");

			drawable.SetBounds (rect.Left, rect.Top, rect.Right, rect.Bottom);
		}

		public static void SetBounds(this Drawable drawable, RectF rect)
		{
			if (drawable == null) throw new ArgumentNullException ("drawable");
			if (rect == null) throw new ArgumentNullException ("rect");

			SetBounds (drawable, new Rect ((int)rect.Left, (int)rect.Top, (int)rect.Right, (int)rect.Bottom));
		}

		public static void Draw(this Drawable drawable, Canvas canvas, Rect rect)
		{
			SetBounds (drawable, rect);
			drawable.Draw (canvas);
		}

		public static void Draw(this Drawable drawable, Canvas canvas, RectF rect)
		{
			SetBounds (drawable, rect);
			drawable.Draw (canvas);
		}
	}
}
