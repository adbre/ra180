Showing howto start a XSockets.NET server

//1: WebApplication, use PreApplicationStartMethod

using System.Web;
[assembly: PreApplicationStartMethod(typeof(YourWebApp.App_Start.XSocketsBootstrap), "Start")]


//Server instance
using XSockets.Core.Common.Socket;

//Create class for the server instance
public static class XSocketsBootstrap
{
    private static IXSocketServerContainer wss;
    public static void Start()
    {
        container = XSockets.Plugin.Framework.Composable.GetExport<IXSocketServerContainer>();
        container.StartServers();
        foreach (var server in container.Servers)
        {
            Debug.WriteLine("Started Server: {0}:{1}", server.ConfigurationSetting.Location, server.ConfigurationSetting.Port);
            Debug.WriteLine("Scheme: {0}", server.ConfigurationSetting.Scheme);
            Debug.WriteLine("SSL/TLS: {0}", server.ConfigurationSetting.IsSecure);
            Debug.WriteLine("Allowed Connections (0 = infinite): {0}", server.ConfigurationSetting.NumberOfAllowedConections);
            Debug.WriteLine("------------------------------------------------------");
        }
    }        
}

//2: Console Application
using (var container = XSockets.Plugin.Framework.Composable.GetExport<IXSocketServerContainer>())
{
    container.StartServers();
    Console.WriteLine("Server started, hit enter to quit");
    Console.ReadLine();
}