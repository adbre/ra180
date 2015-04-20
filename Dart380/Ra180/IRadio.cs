using System;

namespace Ra180
{
    public interface IRadio
    {
        void SetChannelData(Ra180ChannelData channelData, DateTime dateTime);

        event MessageEventHandler Received;
        bool SendDataMessage(MessageEventArgs message, Action callback);
    }
}