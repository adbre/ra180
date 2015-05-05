﻿using System;
using Ra180.Programs;

namespace Ra180
{
	public abstract class Dart380Base : Ra180Device, IDart380
	{
		private readonly Ra180Display _largeDisplay;
		private readonly Ra180Display _smallDisplay;
	    private Ra180Channel _channel = Ra180Channel.Channel1;
	    private Dart380Mod _mod = Dart380Mod.FR;
	    private Ra180Volume _volume = Ra180Volume.Volume4;

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

        public virtual object Tvåtråd { get; set; }
        public virtual object Data { get; set; }
        public virtual object Mik2 { get; set; }
        public virtual object Mik1 { get; set; }

        [Obsolete]
	    public Ra180 Ra180
	    {
            get { return Mik2 as Ra180 ?? Tvåtråd as Ra180; }
	        set { Mik2 = value; }
	    }

	    protected override void ClearDisplay()
	    {
	        _largeDisplay.Clear();
            _smallDisplay.Clear();
	    }
	}
}
