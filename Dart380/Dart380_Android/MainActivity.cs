using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Ra180;
using System.Timers;

namespace Dart380_Android
{
	[Activity (Label = "Dart380_Android", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
	public class MainActivity : Activity
	{
		private const int BlinkingIntervalInMilliseconds = 500;
		private readonly Timer _timer;
		private Dart380View _view;

		public MainActivity ()
		{
			_timer = new Timer (BlinkingIntervalInMilliseconds);
			_timer.Elapsed += (sender, e) =>  {
				if (_view != null) {
					RunOnUiThread(() =>  _view.ToggleBlinking());
				}
			};
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			_view = new Dart380View (this, new FakeDart380());
			SetContentView (_view);

			_timer.Start ();
		}

		protected override void OnStop ()
		{
			_timer.Stop ();
			base.OnStop ();
		}
	}
}


