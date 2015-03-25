using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ra180
{
	public abstract class Dart380Base : IDart380
	{
		private Ra180Display _largeDisplay;
		private Ra180Display _smallDisplay;

		protected Dart380Base ()
		{
			_largeDisplay = new Ra180Display (16);
			_smallDisplay = new Ra180Display (8);
		}

		public Ra180Display LargeDisplay
		{
			get { return _largeDisplay; }
		}

		public Ra180Display SmallDisplay
		{
			get { return _smallDisplay; }
		}

		public abstract void SendKey (string key);
	}
}

