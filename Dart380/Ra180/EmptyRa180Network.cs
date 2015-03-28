using System;

namespace Ra180
{
    public class EmptyRa180Network : IRa180Network
    {
        public event EventHandler ReceivedSynk;

        public virtual void ChannelData(Ra180ChannelData channelData, DateTime dateTime)
        {
        }

        protected virtual void OnReceivedSynk()
        {
            EventHandler handler = ReceivedSynk;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}