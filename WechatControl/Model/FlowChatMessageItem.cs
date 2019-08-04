using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatControl
{
    public class FlowChatMessageItem
    {
        public string MessagerName { get; set; }
        public string ImageUrl { get; set; }
        public Image Image { get; set; }
        public string LastMessageText { get; set; }
        public string LastMessageTime { get; set; }
        internal bool IsFocus { get; set; }
        internal bool IsClick { get; set; }
        internal Rectangle Rect { get; set; }
    }
}
