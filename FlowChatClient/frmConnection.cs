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

namespace FlowChatClient
{
    public partial class frmConnection : Form
    {
        private frmLoadingWin loadingWin;
        private BackgroundWorker connectWorker;
        private TcpClient tcpClient;
        
        public TcpClient TcpClient { get { return tcpClient; } }

        
        public frmConnection()
        {
            InitializeComponent();
            loadingWin = new frmLoadingWin();
            connectWorker = new BackgroundWorker();
            connectWorker.DoWork += Worker_DoWork;
            connectWorker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnConnect.Enabled = true;
            btnCancel.Enabled = true;
            loadingWin.Close();

            object[] arr = e.Result as object[];
            if (arr[0].ToString() == "Failed")
            {
                MessageBox.Show(arr[1].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Hide();
                frmControlPanel controlPanelWin = new frmControlPanel(TcpClient);
                controlPanelWin.ShowDialog();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arr = new object[2];

            try
            {
                tcpClient = new TcpClient(txtIP.Text, int.Parse(txtPort.Text));

                arr[0] = "Success";
                arr[1] = null;
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
            if (!IPAddress.TryParse(txtIP.Text, out _))
            {
                MessageBox.Show("Please input a valid ip address!");
                return;
            }
            else if(!int.TryParse(txtPort.Text, out _))
            {
                MessageBox.Show("Please input a valid port!");
                return;
            }

            btnConnect.Enabled = false;
            btnCancel.Enabled = false;

            loadingWin = new frmLoadingWin();
            loadingWin.Show();
            loadingWin.BringToFront();

            connectWorker.RunWorkerAsync();
        }
    }
}
