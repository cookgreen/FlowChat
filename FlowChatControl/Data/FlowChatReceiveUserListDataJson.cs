using FlowChatControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatControl.Data
{
    public class FlowChatReceiveUserListDataJson : FlowChatReceiveDataJson
    {
        public List<FlowChatUserModel> UserList { get; set; }
    }
}
