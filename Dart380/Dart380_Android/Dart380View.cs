using System;
using Android.Views;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Graphics;
using System.Collections.Generic;
using Android.Widget;
using System.Linq;

namespace Dart380_Android
{
	public class Dart380View : View
	{
		private readonly BitmapDrawable _bitmap;
		private readonly Rect _bitmapOriginalSize;
		private readonly List<HotArea> _hotAreas = new List<HotArea>();

		private Rect _dartRect;
		private float _scale;

		public Dart380View (Context context) : base(context)
		{
			_bitmap = (BitmapDrawable)Resources.GetDrawable (Resource.Drawable.Dart);
			_bitmapOriginalSize = _bitmap.Bitmap.GetSize ();

			_hotAreas.AddRange (GetButtonHotAreas ());
		}

		public override bool OnTouchEvent (MotionEvent e)
		{
			if (base.OnTouchEvent (e))
				return true;

			var x = e.GetX ();
			var y = e.GetY ();

			foreach (var hotArea in _hotAreas) {
				if (hotArea.Contains (x, y)) {
					var callback = hotArea.Callback;
					if (callback != null)
						callback ();
					return true;
				}
			}

			return false;
		}

		protected override void OnDraw (Canvas canvas)
		{
			base.OnDraw (canvas);
			if (_dartRect == null) {
				ConfigureMeasurements(canvas.Width, canvas.Height);
			}

			_bitmap.Draw (canvas, _dartRect);

			DrawHotAreas (canvas);
		}

		protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged (w, h, oldw, oldh);
			ConfigureMeasurements (w, h);
		}

		private void DrawHotAreas(Canvas canvas)
		{
			var hotAreaPaint = new Paint ();
			hotAreaPaint.SetARGB (255, 200, 255, 0);
			hotAreaPaint.SetStyle (Paint.Style.Stroke);
			hotAreaPaint.StrokeWidth = 4;

			foreach (var hotArea in _hotAreas) {
				var drawable = new ShapeDrawable (new RectShape ());
				drawable.Paint.Set (hotAreaPaint);
				drawable.SetBounds (hotArea.Rectangle);
				drawable.Draw (canvas);
			}
		}

		private void ConfigureMeasurements(int w, int h)
		{
			_dartRect = _bitmapOriginalSize.ScaleToFit(w, h);
			_scale = _bitmapOriginalSize.GetScale (w, h);

			foreach (var hotArea in _hotAreas) {
				hotArea.Rectangle = new Rect (
					(int)(_dartRect.Left + hotArea.OriginalRect.Left * _scale),
					(int)(_dartRect.Top + hotArea.OriginalRect.Top * _scale),
					(int)(_dartRect.Left + hotArea.OriginalRect.Right * _scale),
					(int)(_dartRect.Top + hotArea.OriginalRect.Bottom * _scale)
				);
			}
		}

		private IEnumerable<HotArea> GetButtonHotAreas()
		{
			return Dart380Keyboard
				.GetButtons ()
				.Select (btn => new HotArea (btn.Rectangle, () => Toast.MakeText (this.Context, string.Format ("Key={0}", btn.PrimaryKey), ToastLength.Short).Show ()));
		}
	}

	public class HotArea
	{
		public HotArea() {
		}

		public HotArea(Rect originalRectangle, Action callback)
		{
			OriginalRect = originalRectangle;
			Callback = callback;
		}

		public Action Callback { get; set; }
		public Rect OriginalRect { get; set; }
		public Rect Rectangle { get; set; }

		public bool Contains(float x, float y)
		{
			return new Region(Rectangle).Contains((int)x, (int)y);
		}
	}

	public static class Dart380Keyboard
	{
		public static IEnumerable<Dart380Button> GetButtons()
		{
			// The coordinates assumes the Dart380 image is 1363x1018 pixels in size.
			return new List<Dart380Button> {
				new Dart380Button("F1", 143,353, 61,59),
				new Dart380Button("F2", 224,353, 61,59),
				new Dart380Button("F3", 306,353, 61,59),
				new Dart380Button("F4", 388,353, 61,59),
				new Dart380Button("FMT", 470,353, 61,59),
				new Dart380Button("REP", 143,435, 61,59),
				new Dart380Button("RAD", 224,435, 61,59),
				new Dart380Button("KVI", 306,435, 61,59),
				new Dart380Button("SKR", 388,435, 61,59),
				new Dart380Button("DDA", 470,435, 61,59),
				new Dart380Button("SND", 143,516, 61,59),
				new Dart380Button("EKV", 224,516, 61,59),
				new Dart380Button("MOT", 306,516, 61,59),
				new Dart380Button("AVS", 388,516, 61,59),
				new Dart380Button("ISK", 470,516, 61,59),
				new Dart380Button("OPM", 604,435, 61,59),
				new Dart380Button("EFF", 686,435, 61,59),
				new Dart380Button("ÄND", 767,435, 61,59),
				new Dart380Button("BEL", 604,516, 61,59),
				new Dart380Button("SLT", 686,516, 61,59),
				new Dart380Button("ENT", 767,516, 61,59),
				new Dart380Button("1", 879,272, 61,59),
				new Dart380Button("2", 961,272, 61,59),
				new Dart380Button("3", 1042,272, 61,59),
				new Dart380Button("4", 879,353, 61,59),
				new Dart380Button("5", 961,353, 61,59),
				new Dart380Button("6", 1042,353, 61,59),
				new Dart380Button("7", 879,435, 61,59),
				new Dart380Button("8", 961,435, 61,59),
				new Dart380Button("9", 1042,435, 61,59),
				new Dart380Button("*", 879,516, 61,59),
				new Dart380Button("0", 961,516, 61,59),
				new Dart380Button("#", 1042,516, 61,59),
				new Dart380Button("Q", "!", 143,639, 61,59),
				new Dart380Button("W", "\"", 224,639, 61,59),
				new Dart380Button("E", "#", 306,639, 61,59),
				new Dart380Button("R", "@", 388,639, 61,59),
				new Dart380Button("T", "%", 470,639, 61,59),
				new Dart380Button("Y", "&", 551,639, 61,59),
				new Dart380Button("U", "/", 632,639, 61,59),
				new Dart380Button("I", "/", 714,639, 61,59),
				new Dart380Button("O", "(", 796,639, 61,59),
				new Dart380Button("P", ")", 877,639, 61,59),
				new Dart380Button("Å", "+", 958,639, 61,59),
				new Dart380Button("DEL", 1040,639, 61,59),
				new Dart380Button("A", 184,721, 61,59),
				new Dart380Button("S", 266,721, 61,59),
				new Dart380Button("D", 347,721, 61,59),
				new Dart380Button("F", 429,721, 61,59),
				new Dart380Button("G", "´", 510,721, 61,59),
				new Dart380Button("H", "^", 592,721, 61,59),
				new Dart380Button("J", "$", 673,721, 61,59),
				new Dart380Button("K", "<", 755,721, 61,59),
				new Dart380Button("L", ">", 837,721, 61,59),
				new Dart380Button("Ö", "*", 918,721, 61,59),
				new Dart380Button("Ä", "?", 1000,721, 61,59),
				new Dart380Button("SHIFT", 143,802, 61,59),
				new Dart380Button("Z", 224,802, 61,59),
				new Dart380Button("X", 306,802, 61,59),
				new Dart380Button("C", 388,802, 61,59),
				new Dart380Button("V", 470,802, 61,59),
				new Dart380Button("B", 551,802, 61,59),
				new Dart380Button("N", 632,802, 61,59),
				new Dart380Button("M", 714,802, 61,59),
				new Dart380Button(",", ";", 796,802, 61,59),
				new Dart380Button(".", ":", 877,802, 61,59),
				new Dart380Button("-", "_", 958,802, 61,59),
				new Dart380Button("ENT", 1040,802, 61,59),
				new Dart380Button("LARROW", 224,884, 61,59),
				new Dart380Button("RARROW", 306,884, 61,59),
				new Dart380Button("UARROW", 388,884, 61,59),
				new Dart380Button("DARROW", 470,884, 61,59),
				new Dart380Button("SPACE", 551,884, 307,59),
				new Dart380Button("PGUP", 877,884, 61,59),
				new Dart380Button("PGDOWN", 958,884, 61,59),
			};
		}
	}

	public class Dart380Button
	{
		public Dart380Button (string primaryKey, int x, int y, int w, int h)
			: this(primaryKey, null, x, y, w, h)
		{
		}

		public Dart380Button(string primaryKey, string secondaryKey, int x, int y, int w, int h)
		{
			PrimaryKey = primaryKey;
			SecondaryKey = secondaryKey;
			Rectangle = new Rect (x, y, x + w, y + h);
		}

		public string PrimaryKey { get; set; }
		public string SecondaryKey { get; set; }
		public Rect Rectangle { get; set; }
	}
}

