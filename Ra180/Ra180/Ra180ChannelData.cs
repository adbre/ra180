namespace Ra180
{
    public class Ra180ChannelData
    {
        public Ra180ChannelData()
        {
            FR = 00000;
            BD1 = new Ra180Band();
            BD2 = new Ra180Band();
        }

        public Ra180ChannelData(int fr) : this()
        {
            FR = fr;
        }

        public int FR { get; internal set; }
        public Ra180Band BD1 { get; internal set; }
        public Ra180Band BD2 { get; internal set; }
        public bool IsKLARDisabled { get; set; }
        public bool Synk { get; set; }

        public Ra180DataKey PNY { get; internal set; }
        public Ra180DataKey NYK { get; internal set; }
    }

    public class Ra180Band
    {
        public Ra180Band()
        {
            Start = 90;
            End = 00;
        }

        public short Start { get; set; }
        public short End { get; set; }
    }
}