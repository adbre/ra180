namespace Ra180
{
	public abstract class Dart380Base : Ra180Device, IDart380
	{
		private readonly Ra180Display _largeDisplay;
		private readonly Ra180Display _smallDisplay;

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

        public Ra180 Ra180 { get; set; }

	    protected override void ClearDisplay()
	    {
	        _largeDisplay.Clear();
            _smallDisplay.Clear();
	    }
	}
}

