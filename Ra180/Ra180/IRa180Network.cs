using System;

namespace Ra180
{
    public interface IRa180Network
    {
        event EventHandler ReceivedSynk;
        void ChannelData(Ra180ChannelData channelData, DateTime dateTime);
    }
}