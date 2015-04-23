using Microsoft.AspNet.SignalR;

namespace Ra180.Server
{
    public class Ra180Hub : Hub
    {
        public void Send(string message)
        {
            Clients.Others.Receive(message);
            //Clients.All.Receive(message);
        }
    }
}