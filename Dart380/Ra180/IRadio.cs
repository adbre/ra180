using System;
using System.Collections.Generic;
using System.Linq;

namespace Ra180
{
    public interface IRadio
    {
        void SetChannelData(Ra180ChannelData channelData, DateTime dateTime);

        event MessageEventHandler Received;
        bool SendDataMessage(MessageEventArgs message, Action callback);
    }

    public delegate void MessageEventHandler(object sender, MessageEventArgs args);

    public class MessageEventArgs : EventArgs
    {
        private const int MessageRowLength = 16;

        private readonly string _message;
        private readonly IEnumerable<string> _messageArray;

        public MessageEventArgs(string message)
        {
            _message = message;
            _messageArray = Split(message, MessageRowLength);
        }

        public MessageEventArgs(IEnumerable<string> message)
        {
            _messageArray = message.Select(s => s.PadRight(MessageRowLength, ' ')).ToArray();
            _message = string.Join("", _messageArray);
        }

        public string Message { get { return _message; } }
        public string[] MessageArray { get { return _messageArray.ToArray(); } }

        private static IEnumerable<string> Split(string message, int size)
        {
            var index = 0;
            while (index + size < message.Length)
            {
                yield return message.Substring(index, size);
                index += size;
            }

            if (index < message.Length)
                yield return message.Substring(index);
        }
    }
}