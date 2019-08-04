﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatControl
{
    public class FlowChatCheckItem
    {
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public Image Image { get; set; }
        public bool IsChecked { get; set; }
        internal Rectangle Rect { get; set; }
        internal bool IsClick { get; set; }
        internal bool IsFocus { get; set; }
    }
}
