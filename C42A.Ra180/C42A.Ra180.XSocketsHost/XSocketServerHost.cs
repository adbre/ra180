using System.ServiceProcess;
using XSockets.Core.Common.Socket;

namespace C42A.Ra180.XSocketsHost
{
    public partial class XSocketServerHost : ServiceBase
    {
        private IXSocketServerContainer _xSocketServer;

        public XSocketServerHost()
        {
            InitializeComponent();
        }

        private static IXSocketServerContainer CreateXSocketServer()
        {
            return XSockets.Plugin.Framework.Composable.GetExport<IXSocketServerContainer>();
        }

        protected override void OnStart(string[] args)
        {
            _xSocketServer = CreateXSocketServer();
            _xSocketServer.StartServers();
        }

        protected override void OnStop()
        {
            var server = _xSocketServer;
            if (server == null) return;
            server.StopServers();
            server.Dispose();
            _xSocketServer = null;
        }
    }
}
