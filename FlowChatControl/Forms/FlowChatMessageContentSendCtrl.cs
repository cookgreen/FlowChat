using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowChatControl.Forms
{
    public partial class FlowChatMessageContentSendCtrl : UserControl
    {
        public FlowChatMessageContentSendCtrl()
        {
            InitializeComponent();
        }

        public void AddMessageItem(string content, bool isMe)
        {
            FlowChatMessageContentItemModel item = new FlowChatMessageContentItemModel();
            item.MessageText = content;
            item.IsMe = isMe;
            flowChatMessageContentListBox1.AddNewMessage(item);
        }
    }
}
