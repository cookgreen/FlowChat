using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatControl.Model
{
    public class FlowChatReceiveDataJson
    {
        public string Type { get; set; }
        public int Status { get; set; }
        public object Data { get; set; }
    }

    public class FlowChatReceiveDataLoginJson
    {
        public FlowChatUserModel UserData { get; set; }
    }
}
