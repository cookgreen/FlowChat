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
                    FlowChatSendDataJson sendLoginData = serializer.Deserialize(str, typeof(FlowChatSendDataJson)) as FlowChatSendDataJson;
                    if (sendLoginData.Type == "Login")
                    {
                        FlowChatUserModel loginUserData = sendLoginData.UserData;

                        Console.WriteLine("Client " + tcpClient.Client.RemoteEndPoint.ToString() + " loggin to the Server...");

                        if (userDAL.CheckUser(loginUserData.UserName, loginUserData.Password, out loginUserData))
                        {
                            FlowChatReceiveDataJson recvLoginData = new FlowChatReceiveDataJson();
                            recvLoginData.UserData = loginUserData;
                            recvLoginData.Type = "Login";
                            recvLoginData.Status = 1;

                            var sendBuffer = Encoding.UTF8.GetBytes(recvLoginData.ToJSON());
                            stream.Write(sendBuffer, 0, sendBuffer.Length);

                            Console.WriteLine("Client " + tcpClient.Client.RemoteEndPoint.ToString() + " Login Success!");
                        }
                        else
                        {
                            FlowChatReceiveDataJson recvLoginData = new FlowChatReceiveDataJson();
                            recvLoginData.UserData = null;
                            recvLoginData.Type = "Login";
                            recvLoginData.Status = 0;
                            recvLoginData.Message = "Username or Password is incorrect";

                            tcpClient.Client.Send(Encoding.UTF8.GetBytes(recvLoginData.ToJSON()));

                            Console.WriteLine("Client " + tcpClient.Client.RemoteEndPoint.ToString() + " Login Failed!");
                        }
                    }
                    else if (sendLoginData.Type == "Register")
                    {
                        FlowChatUserModel registerUserData = sendLoginData.UserData;
                        if (!userDAL.CheckUser(registerUserData.UserName, out registerUserData)) 
                        {
                            userDAL.AddUser(registerUserData, out registerUserData);

                            FlowChatReceiveDataJson recvRegData = new FlowChatReceiveDataJson();
                            recvRegData.UserData = registerUserData;
                            recvRegData.Type = "Register";
                            recvRegData.Status = 1;

                            tcpClient.Client.Send(Encoding.UTF8.GetBytes(recvRegData.ToJSON()));
                        }
                        else
                        {
                            FlowChatReceiveDataJson recvRegData = new FlowChatReceiveDataJson();
                            recvRegData.UserData = null;
                            recvRegData.Type = "Register";
                            recvRegData.Status = 0;
                            recvRegData.Message = "The User with the same username has already existed!";

                            tcpClient.Client.Send(Encoding.UTF8.GetBytes(recvRegData.ToJSON()));
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
