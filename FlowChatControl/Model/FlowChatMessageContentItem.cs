using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatControl
{
    public class FlowChatMessageContentItem
    {
        public string ImageUrl { get; set; }
        public Image Image { get; set; }
        public string MessageText { get; set; }
        public string MessageTime { get; set; }
        public bool IsMe { get; set; }
        internal Rectangle avatarRect { get; set; }
        internal Rectangle messageRect { get; set; }
    }
}
