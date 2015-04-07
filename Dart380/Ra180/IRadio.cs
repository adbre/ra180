using System;

namespace Ra180
{
    public interface IRadio
    {
        event EventHandler ReceivedSynk;
        void SetChannelData(Ra180ChannelData channelData, DateTime dateTime);
        bool SendDataMessage(string[] data, Action callback);
    }
}