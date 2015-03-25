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

	public class Dart380Control : HotArea
	{
		public Dart380Control (int x, int y, int w, int h)
		{
			OriginalRect = new Rect (x, y, x + w, y + h);
		}
	}

}
