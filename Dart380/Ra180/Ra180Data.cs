namespace Ra180
{
    public class Ra180Data
    {
        private readonly Ra180ChannelDataCollection _channelData = new Ra180ChannelDataCollection();

        public Ra180Data()
        {
            _channelData.Changed += (sender, args) => IsNOLLST = false;
            
            Channel = Ra180Channel.Channel1;
            NOLLST();
        }

        public Ra180Eff Eff { get; internal set; }
        public bool Sdx { get; internal set; }
        public bool Opmtn { get; internal set; }

        public Ra180Channel Channel { get; internal set; }

        public Ra180ChannelDataCollection ChannelData { get { return _channelData; } }

        public Ra180ChannelData CurrentChannelData
        {
            get { return ChannelData[Channel]; }
        }

        public bool IsNOLLST { get; private set; }

        public void NOLLST()
        {
            Eff = Ra180Eff.Låg;
            Sdx = false;
            Opmtn = false;

            ChannelData.NOLLST();
            IsNOLLST = true;
        }
    }
}