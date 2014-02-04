using System;

namespace C42A.Ra180.Infrastructure
{
    public interface IDartTransport
    {
        event EventHandler<string> ReceivedString;
        void SendString(string s);
    }
}