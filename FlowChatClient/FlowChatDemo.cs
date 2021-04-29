using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WechatControl.Forms;

namespace WechatControl.Test
{
    public partial class FlowChatDemo : Form
    {
        

        public FlowChatDemo()
        {
            InitializeComponent();

            flowChatMessageListBox1.SelectedMessageItemChanged += FlowChatMessageListBox1_SelectedMessageItemChanged;

            flowChatMessageListBox1.Items.Add(new FlowChatMessageItem()
            {
                ImageUrl = "avatar.png",
                MessagerName = "发消息的人",
            });
        }

        private void FlowChatMessageListBox1_SelectedMessageItemChanged(object arg1, FlowChatMessageItem arg2)
        {
            FlowChatMessageContentSendCtrl flowChatMessageContentItem = new FlowChatMessageContentSendCtrl();
            splitContainer1.Panel2.Controls.Clear();
            flowChatMessageContentItem.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(flowChatMessageContentItem);
        }
    }
}
