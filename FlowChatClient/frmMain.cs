using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowChatControl;
using FlowChatControl.Forms;
using FlowChatControl.Model;

namespace FlowChatClient
{
    public partial class frmMain : Form
    {
        private FlowChatSession session;

        public frmMain(FlowChatSession session)
        {
            InitializeComponent();

            this.session = session;

            flowChatMessageListBox1.SelectedMessageItemChanged += FlowChatMessageListBox1_SelectedMessageItemChanged;

            flowChatMessageListBox1.Items.Add(new FlowChatMessageItemModel()
            {
                ImageUrl = "avatar.png",
                MessagerName = "发消息的人",
            });
        }

        private void FlowChatMessageListBox1_SelectedMessageItemChanged(object arg1, FlowChatMessageItemModel arg2)
        {
            FlowChatMessageContentSendCtrl flowChatMessageContentItem = new FlowChatMessageContentSendCtrl();
            splitContainer1.Panel2.Controls.Clear();
            flowChatMessageContentItem.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(flowChatMessageContentItem);
        }
    }
}
