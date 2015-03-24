using System;

namespace Ra180
{
    public class Ra180ChannelDataCollection
    {
        private readonly Ra180ChannelData[] _channels = new Ra180ChannelData[8];

        public Ra180ChannelDataCollection()
        {
            this[Ra180Channel.Channel1] = new Ra180ChannelData();
        }

        public Ra180ChannelData this[Ra180Channel channel]
        {
            get { return _channels[ToIndex(channel)]; }
            internal set { _channels[ToIndex(channel)] = value; }
        }

        private static int ToIndex(Ra180Channel channel)
        {
            switch (channel)
            {
                case Ra180Channel.Channel1: return 0;
                case Ra180Channel.Channel2: return 1;
                case Ra180Channel.Channel3: return 2;
                case Ra180Channel.Channel4: return 3;
                case Ra180Channel.Channel5: return 4;
                case Ra180Channel.Channel6: return 5;
                case Ra180Channel.Channel7: return 6;
                case Ra180Channel.Channel8: return 7;
                default:
                    throw new ArgumentOutOfRangeException("channel");
            }
        }

        public void NOLLST()
        {
            this[Ra180Channel.Channel1] = new Ra180ChannelData(30025);
            this[Ra180Channel.Channel2] = new Ra180ChannelData(40025);
            this[Ra180Channel.Channel3] = new Ra180ChannelData(50025);
            this[Ra180Channel.Channel4] = new Ra180ChannelData(60025);
            this[Ra180Channel.Channel5] = new Ra180ChannelData(70025);
            this[Ra180Channel.Channel6] = new Ra180ChannelData(80025);
            this[Ra180Channel.Channel7] = new Ra180ChannelData(87975);
            this[Ra180Channel.Channel8] = new Ra180ChannelData(42025);
        }
    }
}