using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WechatControl;
using WechatControl.Test.Properties;

namespace WechatControl.Test
{
    public partial class TestApp : Form
    {
        public TestApp()
        {
            InitializeComponent();

            weChatFlowListBox1.Items.Add(new FlowChatMessageItem()
            {
                MessagerName = "xxxx1",
                LastMessageText = "[Files]",
                Image = Resources.avatar,
                LastMessageTime = "17:14"
            });
            weChatFlowListBox1.Items.Add(new FlowChatMessageItem()
            {
                MessagerName = "xxxx2",
                LastMessageText = "[Files]",
                Image = Resources.avatar,
                LastMessageTime = "17:14"
            });
            weChatFlowListBox1.Items.Add(new FlowChatMessageItem()
            {
                MessagerName = "xxxx3",
                LastMessageText = "[Files]",
                Image = Resources.avatar,
                LastMessageTime = "17:14"
            });
            weChatFlowListBox1.Items.Add(new FlowChatMessageItem()
            {
                MessagerName = "xxxx4",
                LastMessageText = "[Files]",
                Image = Resources.avatar,
                LastMessageTime = "17:14"
            });
            weChatFlowChatContent1.ChatItems.Add(
                new FlowChatMessageContentItem()
                {
                    Image = Resources.avatar,
                    MessageText = "11"
                }
            );
            weChatFlowChatContent1.ChatItems.Add(
                new FlowChatMessageContentItem()
                {
                    Image = Resources.avatar,
                    MessageText = "11"
                });
            weChatFlowChatContent1.ChatItems.Add(
                new FlowChatMessageContentItem()
                {
                    Image = Resources.avatar,
                    MessageText = "11"
                });
            weChatFlowChatContent1.ChatItems.Add(
                new FlowChatMessageContentItem()
                {
                    Image = Resources.avatar,
                    MessageText = "11",
                    IsMe = true
                });
            weChatFlowChatContent1.ChatItems.Add(
                new FlowChatMessageContentItem()
                {
                    Image = Resources.avatar,
                    MessageText = "11"
                });
            weChatAddressList1.AddUser(
                new FlowChatItem()
                {
                    ImageUrl = @"C:\Users\Administrator\Desktop\avatar.jpg",
                    Text = "A"
                });
            weChatAddressList1.AddUser(
                new FlowChatItem()
                {
                    ImageUrl = @"C:\Users\Administrator\Desktop\avatar.jpg",
                    Text = "B"
                });
            weChatAddressList1.AddUser(
                new FlowChatItem()
                {
                    ImageUrl = @"C:\Users\Administrator\Desktop\avatar.jpg",
                    Text = "C"
                });
            weChatAddressList1.AddUser(
                new FlowChatItem()
                {
                    ImageUrl = @"C:\Users\Administrator\Desktop\avatar.jpg",
                    Text = "AA"
                });
            weChatAddressList1.AddUser(
                new FlowChatItem()
                {
                    ImageUrl = @"C:\Users\Administrator\Desktop\avatar.jpg",
                    Text = "DD"
                });
            weChatAddressList1.AddUser(
                new FlowChatItem()
                {
                    ImageUrl = @"C:\Users\Administrator\Desktop\avatar.jpg",
                    Text = "啊Q"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    ImageUrl = @"C:\Users\Administrator\Desktop\avatar.jpg",
                    Text = "A"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    ImageUrl = @"C:\Users\Administrator\Desktop\avatar.jpg",
                    Text = "S"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "D"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "F"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "F"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "D"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "PP"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "PP"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    ImageUrl = @"C:\Users\Administrator\Desktop\avatar.jpg",
                    Text = "XX"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "MM"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "Zoo"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "Zoo"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "Zoo"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "Zoo"
                });
            flowChatCheckAddressList1.AddItem(
                new FlowChatCheckItem()
                {
                    Image = Resources.avatar,
                    Text = "Zoo"
                });
        }
    }
}
