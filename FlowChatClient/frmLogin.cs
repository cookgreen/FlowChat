using FlowChatControl.Forms;
using FlowChatControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using FlowChatControl;
using FlowChatControl.Data;

namespace FlowChatClient
{
    public partial class frmLogin : Form
    {
        private frmLoadingWin loadingWin;
        private BackgroundWorker loginWorker;
        private TcpClient tcpClient;
        private FlowChatSession session;
        public FlowChatSession Session { get { return session; } }
        
        public frmLogin(TcpClient tcpClient)
        {
            InitializeComponent();

            this.tcpClient = tcpClient;

            loadingWin = new frmLoadingWin();
            loginWorker = new BackgroundWorker();
            loginWorker.DoWork += Worker_DoWork;
            loginWorker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnLogin.Enabled = true;
            btnCancel.Enabled = true;
            loadingWin.Close();

            object[] arr = e.Result as object[];
            if (arr[0].ToString() == "Failed")
            {
                MessageBox.Show(arr[1].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arr = new object[2];

            try
            {
                FlowChatUserModel userModel = new FlowChatUserModel();
                userModel.UserName = txtUsername.Text;
                userModel.Password = txtPassword.Text;

                FlowChatSendLoginDataJson sendLoginDataJson = new FlowChatSendLoginDataJson();
                sendLoginDataJson.Type = "Login";
                sendLoginDataJson.Data = userModel;

                string jsonStr = sendLoginDataJson.ToJSON();
                jsonStr = FlowChatConsts.NETWORK_SEND_DATA_LOGIN_REGISTER_PREFIX + "|" + jsonStr;

                NetworkStream stream = tcpClient.GetStream();
                var buffer = Encoding.UTF8.GetBytes(jsonStr);
                stream.Write(buffer, 0, buffer.Length);

                System.Threading.Thread.Sleep(1000);

                buffer = new byte[3096];
                int len = stream.Read(buffer, 0, buffer.Length);
                byte[] newBuffer = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    newBuffer[i] = buffer[i];
                }

                var str = Encoding.UTF8.GetString(newBuffer);
                string[] tokens = str.Split('|');
                switch(tokens[0])
                {
                    case FlowChatConsts.NETWORK_RECV_DATA_LOGIN_REGISTER_PREFIX:
                        FlowChatReceiveLoginDataJson recvLoginData = JsonConvert.DeserializeObject<FlowChatReceiveLoginDataJson>(tokens[1]);
                        if (recvLoginData.Status == 1)
                        {
                            var useData = recvLoginData.Data;
                            session = new FlowChatSession(tcpClient, useData);

                            arr[0] = "Success";
                            arr[1] = null;
                        }
                        else
                        {
                            arr[0] = "Failed";
                            arr[1] = "Login Failed!";
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                arr[0] = "Failed";
                arr[1] = ex.Message;
            }

            e.Result = arr;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Please input a valid username!");
                return;
            }
            else if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please input a valid password!");
                return;
            }

            btnLogin.Enabled = false;
            btnCancel.Enabled = false;

            loadingWin = new frmLoadingWin();
            loadingWin.Show();
            loadingWin.BringToFront();

            loginWorker.RunWorkerAsync();
        }
    }
}
