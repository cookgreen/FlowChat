using FlowChatControl.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatControl
{
    public class FlowChatMessageItemModel
    {
        public string MessagerName { get; set; }
        public string ImageUrl { get; set; }
        public string LastMessageText { get; set; }
        public string LastMessageTime { get; set; }
        public FlowChatUserModel User { get; set; }

        //Render
        internal bool IsFocus { get; set; }
        internal bool IsClick { get; set; }
        internal Rectangle Rect { get; set; }
    }
}
