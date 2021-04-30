using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatControl
{
    public static class FlowChatConsts
    {
        //Send
        public const string NETWORK_SEND_DATA_LOGIN_REGISTER_PREFIX = "LOGIN_REGISTER_SEND_DATA";
        public const string NETWORK_SEND_DATA_MESSAGE = "MESSAGE_SEND_DATA";
        public const string NETWORK_SEND_DATA_REQUEST_USER_LIST = "REQUEST_USER_LIST_SEND_DATA";
        
        //Recv
        public const string NETWORK_RECV_DATA_LOGIN_REGISTER_PREFIX = "LOGIN_REGISTER_RECV_DATA";
        public const string NETWORK_RECV_DATA_REQUEST_USER_LIST = "REQUEST_USER_LIST_RECV_DATA";
    }
}
