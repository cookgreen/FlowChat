using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatControl
{
    public class FlowChatMessageContentItemModel
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
