using System;

namespace Ra180
{
    public interface IRadioFactory
    {
        IRadio Create();
    }

    public class SimpleRadioFactory: IRadioFactory
    {
        private readonly Func<IRadio> _factory;

        public SimpleRadioFactory(Func<IRadio> factory)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            _factory = factory;
        }

        public IRadio Create()
        {
            return _factory();
        }
    }

    public interface IRadio : IDisposable
    {
        void Start();
        void Stop();

        void SetChannelData(Ra180ChannelData channelData, DateTime dateTime);

        event MessageEventHandler Received;
        bool SendDataMessage(MessageEventArgs message, Action callback);
    }
}