using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Dart380_Android
{
	[Activity (Label = "Dart380_Android", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (new Dart380View(this));
		}
	}
}


