using System;
using Android.Views;
using Android.Graphics;

namespace Dart380_Android
{
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

		public void Scale(float scale, Rect area)
		{
			Rectangle = OriginalRect.Scale (scale).PlaceInside (area);
		}

		protected virtual void OnTouchEventDown()
		{
			var handler = Callback;
			if (handler != null)
				handler ();
		}

		protected virtual void OnTouchEventUp()
		{
		}

		public virtual void OnTouchEvent(MotionEvent e)
		{
			var contains = new Region (Rectangle).Contains ((int)e.GetX (), (int)e.GetY ());
			if (contains && e.Action == MotionEventActions.Down)
				OnTouchEventDown ();
			else
				OnTouchEventUp ();
		}
	}
}
