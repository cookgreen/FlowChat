using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatControl.NetworkData
{
    public class FlowChatSendMessageData
    {
        public string SenderUserName { get; set; }
        public string ReceiveUserName { get; set; }
        public DateTime SendTime { get; set; }
        public string Content { get; set; }
    }
}
