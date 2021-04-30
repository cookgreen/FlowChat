using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServerApp serverApp = new ServerApp();
            serverApp.Run();
        }
    }
}
