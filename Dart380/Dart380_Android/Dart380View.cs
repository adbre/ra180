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

