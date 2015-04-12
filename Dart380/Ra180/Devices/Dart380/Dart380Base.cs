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

	    public virtual Ra180Channel Channel
	    {
	        get { return _channel; }
	        set { _channel = value; }
	    }

	    public virtual Dart380Mod Mod
	    {
	        get { return _mod; }
	        set { _mod = value; }
	    }

	    public virtual Ra180Volume Volume
	    {
	        get { return _volume; }
	        set { _volume = value; }
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

