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
    public partial class frmControlPanel : Form
    {
        private TcpClient tcpClient;

        public frmControlPanel(TcpClient tcpClient)
        {
            InitializeComponent();
            this.tcpClient = tcpClient;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Hide();
            frmLogin loginWin = new frmLogin(tcpClient);
            if (loginWin.ShowDialog() == DialogResult.OK)
            {
                var session = loginWin.Session;
                frmMain mainWindow = new frmMain(session);
                mainWindow.Show();
            }
            else
            {
                Show();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Hide();
            frmRegister registerWin = new frmRegister(tcpClient);
            if (registerWin.ShowDialog() == DialogResult.OK)
            {
                //var userData = registerWin.UserData;
                //frmMain mainWindow = new frmMain(userData);
                //mainWindow.Show();
            }
            else
            {
                Show();
            }
        }

        private void frmControlPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
