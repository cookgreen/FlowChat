using FlowChatControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatClient
{
    public class FlowChatSession
    {
        private TcpClient tcpClient;
        private FlowChatUserModel userData;

        public TcpClient TcpClient { get { return tcpClient; } }
        public FlowChatUserModel UserData { get { return userData; } }

        public FlowChatSession(TcpClient tcpClient, FlowChatUserModel userData)
        {
            this.tcpClient = tcpClient;
            this.userData = userData;
        }

        public void End()
        {
            tcpClient.Close();
        }
    }
}
