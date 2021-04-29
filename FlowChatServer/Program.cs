using System;

namespace FlowChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerApp serverApp = new ServerApp();
            serverApp.Run();
        }
    }
}
