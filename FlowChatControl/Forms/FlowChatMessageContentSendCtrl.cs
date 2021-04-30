using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowChatControl.NetworkData;
using Newtonsoft.Json;

namespace FlowChatControl.Forms
{
    public partial class FlowChatMessageContentSendCtrl : UserControl
    {
        private FlowChatSession session;
        private FlowChatMessageItemModel messageItem;

        public FlowChatMessageContentSendCtrl(FlowChatSession session, FlowChatMessageItemModel messageItem)
        {
            InitializeComponent();
            this.session = session;
            this.messageItem = messageItem;
        }

        public void AddMessageItem(string content, bool isMe)
        {
            FlowChatMessageContentItemModel item = new FlowChatMessageContentItemModel();
            item.MessageText = content;
            item.IsMe = isMe;
            flowChatMessageContentListBox1.AddNewMessage(item);
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Modifiers)
            {
                case Keys.Enter:
                    if (!string.IsNullOrEmpty(txtMessage.Text))
                    {
                        txtMessage.Text = null;

                        FlowChatSendMessageData messageData = new FlowChatSendMessageData();
                        messageData.ReceiveUserName = session.UserData.UserName;
                        messageData.SenderUserName = messageItem.MessagerName;
                        messageData.Content = txtMessage.Text;
                        messageData.SendTime = DateTime.Now;

                        string jsonstr = JsonConvert.SerializeObject(messageData);
                        jsonstr = FlowChatConsts.NETWORK_SEND_DATA_MESSAGE + "|" + jsonstr;
                        session.TcpClient.Client.Send(Encoding.UTF8.GetBytes(jsonstr));

                        AddMessageItem(txtMessage.Text, true);
                    }
                    break;
            }
        }
    }
}
