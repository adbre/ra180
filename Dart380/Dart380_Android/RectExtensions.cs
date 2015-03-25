using System;
using Android.Views;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Graphics;

namespace Dart380_Android
{

	public static class RectExtensions
	{
		public static Rect PlaceInside(this Rect src, Rect container)
		{
			return new Rect (
				container.Left + src.Left,
				container.Top + src.Top,
				container.Left + src.Right,
				container.Top + src.Bottom
			);
		}

		public static Rect Scale(this Rect src, float scale)
		{
			return new Rect ((int)(src.Left * scale), (int)(src.Top * scale), (int)(src.Right * scale), (int)(src.Bottom * scale));
		}

		public static Rect ScaleToFit(this Rect src, int dstWidth, int dstHeight)
		{
			return ScaleToFit (src, new Rect (0, 0, dstWidth, dstHeight));
		}

		public static Rect ScaleToFit(this Rect src, Rect dst)
		{
			var scale = GetScale(src, dst);
			var width = src.Width() * scale;
			var height = src.Height() * scale;
			var ymargin = (dst.Height() - height) / 2;
			var xmargin = (dst.Width() - width) / 2;
			return new Rect ((int)xmargin, (int)ymargin, (int)width + (int)xmargin, (int)height + (int)ymargin);
		}

		public static float GetScale(this Rect src, int dstWidth, int dstHeight)
		{
			return GetScale (src, new Rect (0, 0, dstWidth, dstHeight));
		}

		public static float GetScale(this Rect src, Rect dst)
		{
			return Math.Min (dst.Width () / (float)src.Width (), dst.Height () / (float)src.Height ());
		}
	}

}
