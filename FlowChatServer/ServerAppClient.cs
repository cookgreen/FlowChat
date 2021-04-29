using FlowChatControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace FlowChatServer
{
    public class ServerAppClient
    {
        private TcpClient tcpClient;
        private BackgroundWorker worker;
        private UserDAL userDAL;
        public event Action Exited;
        public TcpClient TcpClient { get { return tcpClient; } }

        public ServerAppClient(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();

            userDAL = new UserDAL();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                try
                {
                    NetworkStream stream = tcpClient.GetStream();

                    byte[] buffer = new byte[1024];
                    int length = stream.Read(buffer, 0, buffer.Length);
                    byte[] newBuffer = new byte[length];
                    for (int i = 0; i < length; i++)
                    {
                        newBuffer[i] = buffer[i];
                    }
                    string str = Encoding.UTF8.GetString(newBuffer);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    FlowChatSendDataJson sendData = serializer.Deserialize(str, typeof(FlowChatSendDataJson)) as FlowChatSendDataJson;
                    if (sendData.Type == "Login")
                    {
                        FlowChatSendDataLoginJson sendLoginData = sendData.Data as FlowChatSendDataLoginJson;
                        FlowChatUserModel loginUserData = sendLoginData.UserData;

                        Console.WriteLine("Client " + tcpClient.Client.RemoteEndPoint.ToString() + " loggin to the Server...");

                        if (userDAL.CheckUser(loginUserData.UserName, loginUserData.Password, out loginUserData))
                        {
                            FlowChatReceiveDataLoginJson recvLoginData = new FlowChatReceiveDataLoginJson();
                            recvLoginData.UserData = loginUserData;

                            FlowChatReceiveDataJson recvData = new FlowChatReceiveDataJson();
                            recvData.Type = "Login";
                            recvData.Status = 1;
                            recvData.Data = recvLoginData;

                            var sendBuffer = Encoding.UTF8.GetBytes(recvData.ToJSON());
                            stream.Write(sendBuffer, 0, sendBuffer.Length);

                            Console.WriteLine("Client " + tcpClient.Client.RemoteEndPoint.ToString() + " Login Success!");
                        }
                        else
                        {
                            FlowChatReceiveDataLoginJson recvLoginData = new FlowChatReceiveDataLoginJson();
                            recvLoginData.UserData = null;

                            FlowChatReceiveDataJson recvData = new FlowChatReceiveDataJson();
                            recvData.Type = "Login";
                            recvData.Status = 0;
                            recvData.Data = recvLoginData;

                            tcpClient.Client.Send(Encoding.UTF8.GetBytes(recvData.ToJSON()));

                            Console.WriteLine("Client " + tcpClient.Client.RemoteEndPoint.ToString() + " Login Failed!");
                        }
                    }
                }
                catch
                {
                    Exited?.Invoke();
                    break;
                }
            }
        }
    }
}
