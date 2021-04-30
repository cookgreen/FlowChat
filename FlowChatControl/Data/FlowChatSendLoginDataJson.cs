using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatControl.Model
{
    public class FlowChatSendLoginDataJson
    {
        public string Type { get; set; }
        public FlowChatUserModel Data { get; set; }
    }
}
