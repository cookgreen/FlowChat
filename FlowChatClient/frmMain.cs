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
using FlowChatControl.NetworkData;
using FlowChatControl.Forms;
using FlowChatControl.Model;
using Newtonsoft.Json;

namespace FlowChatClient
{
    public partial class frmMain : Form
    {
        private FlowChatSession session;
        private BackgroundWorker worker;
        private Timer refreshUserListTimer;

        public frmMain(FlowChatSession session)
        {
            InitializeComponent();

            this.session = session;

            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();

            refreshUserListTimer = new Timer();
            refreshUserListTimer.Tick += Timer_Tick;
            refreshUserListTimer.Interval = 100;
            refreshUserListTimer.Start();

            flowChatMessageListBox1.SelectedMessageItemChanged += FlowChatMessageListBox1_SelectedMessageItemChanged;

            RequestUserListExcludeCurrentUserFromServer();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RequestUserListExcludeCurrentUserFromServer();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    var tcpClient = session.TcpClient;
                    var stream = tcpClient.GetStream();

                    byte[] bytes = new byte[3096];
                    int len = stream.Read(bytes, 0, bytes.Length);
                    byte[] newBuffer = new byte[len];
                    for (int i = 0; i < len; i++)
                    {
                        newBuffer[i] = bytes[i];
                    }
                    string str = Encoding.UTF8.GetString(newBuffer);
                    string[] tokens = str.Split('|');
                    switch (tokens[0])
                    {
                        case FlowChatConsts.NETWORK_RECV_DATA_REQUEST_USER_LIST:
                            ParseUserList(tokens[1]);
                            break;
                        case FlowChatConsts.NETWORK_RECV_DATA_USER_DISCONNECT:
                            RemoveUserFromList(tokens[1]);
                            break;
                        case FlowChatConsts.NETWORK_RECV_DATA_USER_MESSAGE:
                            NewMessageArried(tokens[1]);
                            break;
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void ParseUserList(string jsonStr)
        {
            FlowChatReceiveUserListDataJson recvUserListData = JsonConvert.DeserializeObject<FlowChatReceiveUserListDataJson>(jsonStr);
            foreach (var userModel in recvUserListData.UserList)
            {
                if (flowChatMessageListBox1.Items.Where(o => o.MessagerName == userModel.UserName).Count() == 0)
                {
                    FlowChatMessageItemModel messageItem = new FlowChatMessageItemModel();
                    messageItem.ImageUrl = userModel.Avatar;
                    messageItem.MessagerName = userModel.UserName;
                    messageItem.User = userModel;
                    flowChatMessageListBox1.AddMessageItem(messageItem);
                }
            }
        }

        private void RemoveUserFromList(string jsonStr)
        {
            FlowChatReceiveLoginDataJson recvUserData = JsonConvert.DeserializeObject<FlowChatReceiveLoginDataJson>(jsonStr);
            var userModel = recvUserData.Data;
            var messageItem = flowChatMessageListBox1.Items.Where(o => o.MessagerName == userModel.UserName).FirstOrDefault();
            flowChatMessageListBox1.RemoveMessageItem(messageItem);
        }

        private void RequestUserListExcludeCurrentUserFromServer()
        {
            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                var tcpClient = session.TcpClient;
                var stream = tcpClient.GetStream();

                var bytes = Encoding.UTF8.GetBytes(FlowChatConsts.NETWORK_SEND_DATA_REQUEST_USER_LIST);
                stream.Write(bytes, 0, bytes.Length);
            });
            thread.Start();
        }

        private void NewMessageArried(string jsonStr)
        {
            FlowChatSendMessageData sendMessageData = JsonConvert.DeserializeObject<FlowChatSendMessageData>(jsonStr);
            if (flowChatMessageListBox1.CurrentSelectedItem.MessagerName == sendMessageData.SenderUserName)
            {
                ((FlowChatMessageContentSendCtrl)splitContainer1.Panel2.Controls[0]).AddMessageItem(sendMessageData.Content, false);
            }
        }

        private void FlowChatMessageListBox1_SelectedMessageItemChanged(object arg1, FlowChatMessageItemModel arg2)
        {
            FlowChatMessageContentSendCtrl flowChatMessageContentItem = new FlowChatMessageContentSendCtrl();
            splitContainer1.Panel2.Controls.Clear();
            flowChatMessageContentItem.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(flowChatMessageContentItem);
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(FlowChatConsts.NETWORK_SEND_DATA_DISCONNECT);
            session.TcpClient.GetStream().Write(buffer, 0, buffer.Length);

            session.End();
            Application.Exit();
        }


    }
}
