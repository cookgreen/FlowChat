using FlowChatControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowChatClient
{
    public partial class frmRegister : Form
    {
        private TcpClient tcpClient;
        private BackgroundWorker registerWorker;
        private FlowChatSession session;
        public FlowChatSession Session { get { return session; } }

        public frmRegister(TcpClient tcpClient)
        {
            InitializeComponent();

            this.tcpClient = tcpClient;

            registerWorker = new BackgroundWorker();
            registerWorker.DoWork += RegisterWorker_DoWork;
            registerWorker.RunWorkerCompleted += RegisterWorker_RunWorkerCompleted;
        }

        private void RegisterWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void RegisterWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            registerWorker.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
