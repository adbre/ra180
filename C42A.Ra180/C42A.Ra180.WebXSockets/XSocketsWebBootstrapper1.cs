using System.Web;
using XSockets.Core.Common.Socket;

[assembly: PreApplicationStartMethod(typeof(C42A.Ra180.WebXSockets.XSocketsWebBootstrapper1), "Start")]

namespace C42A.Ra180.WebXSockets
{
    public static class XSocketsWebBootstrapper1
    {
        private static IXSocketServerContainer wss;
        public static void Start()
        {
            wss = XSockets.Plugin.Framework.Composable.GetExport<IXSocketServerContainer>();
            wss.StartServers();
        }
    }
}
