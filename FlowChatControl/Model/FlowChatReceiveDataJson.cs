using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatControl.Model
{
    public class FlowChatReceiveDataLoginJson
    {
        public string Type { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public FlowChatUserModel UserData { get; set; }
    }
}
