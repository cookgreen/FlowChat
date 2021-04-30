using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatControl.Model
{
    public class FlowChatSendDataLoginJson
    {
        public string Type { get; set; }
        public FlowChatUserModel UserData { get; set; }
    }
}
