using System;
using System.Collections.Generic;
using System.Linq;

namespace Ra180
{
    public class FakeRadio : EmptyRadio
    {
        private readonly object _syncroot = new object();
        private readonly Queue<MessageItem> _messages = new Queue<MessageItem>();
        private int _queueSize = 1;

        public int QueueSize
        {
            get { return _queueSize; }
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException("value", "QueueSize must be greater than zero.");
                _queueSize = value;
            }
        }

        public Message PendingMessage
        {
            get
            {
                lock (_syncroot)
                {
                    if (_messages.Count <= 0)
                        return null;

                    var item = _messages.Peek();
                    return new Message(item, this);
                }
            }
        }

        public override bool SendDataMessage(string[] data, Action callback)
        {
            lock (_syncroot)
            {
                if (_messages.Count >= _queueSize)
                    return false;

                var item = new MessageItem
                {
                    Data = data,
                    Callback = callback
                };

                _messages.Enqueue(item);
            }

            return base.SendDataMessage(data, callback);
        }

        private void MarkAsSent(MessageItem message)
        {
            lock (_syncroot)
            {
                if (!_messages.Any() || !ReferenceEquals(_messages.Peek(), message))
                    throw new InvalidOperationException("Given message is not the next message to be sent.");

                _messages.Dequeue();

                var callback = message.Callback;
                if (callback != null)
                    callback();
            }
        }

        public class Message
        {
            private readonly MessageItem _item;
            private readonly FakeRadio _radio;
            private readonly string[] _data;

            internal Message(MessageItem item, FakeRadio radio)
            {
                _item = item;
                _radio = radio;
                _data = item.Data;
            }

            public string[] Data
            {
                get { return _data; }
            }

            public void MarkAsSent()
            {
                _radio.MarkAsSent(_item);
            }
        }

        internal class MessageItem
        {
            public string[] Data { get; set; }
            public Action Callback { get; set; }
        }
    }
}