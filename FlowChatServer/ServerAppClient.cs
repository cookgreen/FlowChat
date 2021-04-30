using FlowChatControl;
using FlowChatControl.NetworkData;
using FlowChatControl.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatServer
{
    public class ServerAppClient
    {
        private TcpClient tcpClient;
        private BackgroundWorker worker;
        private UserDAL userDAL;
        private FlowChatUserModel loginedUserData;
        private ServerApp serverApp;

        public event Action Exited;
        public TcpClient TcpClient { get { return tcpClient; } }
        public bool isLogined { get { return loginedUserData != null; } }

        public ServerAppClient(TcpClient tcpClient, ServerApp serverApp)
        {
            this.tcpClient = tcpClient;
            this.serverApp = serverApp;

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
                    string[] tokens = str.Split('|');
                    switch (tokens[0])
                    {
                        case FlowChatConsts.NETWORK_SEND_DATA_LOGIN_REGISTER_PREFIX:
                            ParseLoginDataAndReturn(tokens[1], stream);
                            break;
                        case FlowChatConsts.NETWORK_SEND_DATA_MESSAGE:
                            ParseMessageData(tokens[1], stream);
                            break;
                        case FlowChatConsts.NETWORK_SEND_DATA_REQUEST_USER_LIST:
                            ReturnUserList(stream);
                            break;
                        case FlowChatConsts.NETWORK_SEND_DATA_DISCONNECT:
                            SendDisconnectInfoToOtherClients(stream);
                            break;
                    }
                }
                catch
                {
                    Exited?.Invoke();
                    break;
                }
            }
        }

        private void ParseMessageData(string jsonStr, NetworkStream stream)
        {
            FlowChatSendMessageData sendMessageData = JsonConvert.DeserializeObject<FlowChatSendMessageData>(jsonStr);

            var findedClient = serverApp.ConnectedClients.Where(o => o.isLogined && o.loginedUserData.UserName == sendMessageData.ReceiveUserName).FirstOrDefault();
            if (findedClient != null)
            {
                findedClient.SendMessage(FlowChatConsts.NETWORK_RECV_DATA_USER_MESSAGE + "|" + jsonStr, stream);
            }
            else
            {
                SendMessage(FlowChatConsts.NETWORK_RECV_DATA_ERROR + "|Send Message Error, User can not be reached!", stream);
            }
        }

        private void SendDisconnectInfoToOtherClients(NetworkStream stream)
        {
            var otherClients = getOtherClients();
            foreach (var otherClient in otherClients)
            {
                FlowChatReceiveLoginDataJson recvLoginData = new FlowChatReceiveLoginDataJson();
                recvLoginData.Data = loginedUserData;
                recvLoginData.Type = "Disconnect";
                recvLoginData.Status = 1;

                otherClient.SendMessage(
                    FlowChatConsts.NETWORK_RECV_DATA_USER_DISCONNECT + JsonConvert.SerializeObject(recvLoginData),
                    stream);
            }

            Exited?.Invoke();
        }

        private void SendMessage(string data, NetworkStream stream)
        {
            var sendBuffer = Encoding.UTF8.GetBytes(data);
            stream.Write(sendBuffer, 0, sendBuffer.Length);
        }

        private void ParseLoginDataAndReturn(string jsonStr, NetworkStream stream)
        {
            FlowChatSendLoginDataJson sendLoginData = JsonConvert.DeserializeObject<FlowChatSendLoginDataJson>(jsonStr);
            if (sendLoginData.Type == "Login")
            {
                FlowChatUserModel loginUserData = sendLoginData.Data as FlowChatUserModel;

                Console.WriteLine("Client " + tcpClient.Client.RemoteEndPoint.ToString() + " loggin to the Server...");

                if (userDAL.CheckUser(loginUserData.UserName, loginUserData.Password, out loginUserData))
                {
                    FlowChatReceiveLoginDataJson recvLoginData = new FlowChatReceiveLoginDataJson();
                    recvLoginData.Data = loginUserData;
                    recvLoginData.Type = "Login";
                    recvLoginData.Status = 1;

                    var sendBuffer = Encoding.UTF8.GetBytes(FlowChatConsts.NETWORK_RECV_DATA_LOGIN_REGISTER_PREFIX + "|" + recvLoginData.ToJSON());
                    stream.Write(sendBuffer, 0, sendBuffer.Length);

                    loginedUserData = loginUserData;

                    Console.WriteLine("Client " + tcpClient.Client.RemoteEndPoint.ToString() + " Login Success!");
                }
                else
                {
                    FlowChatReceiveLoginDataJson recvLoginData = new FlowChatReceiveLoginDataJson();
                    recvLoginData.Data = null;
                    recvLoginData.Type = "Login";
                    recvLoginData.Status = 0;
                    recvLoginData.Message = "Username or Password is incorrect";

                    tcpClient.Client.Send(Encoding.UTF8.GetBytes(FlowChatConsts.NETWORK_RECV_DATA_LOGIN_REGISTER_PREFIX + "|" + recvLoginData.ToJSON()));

                    loginedUserData = null;

                    Console.WriteLine("Client " + tcpClient.Client.RemoteEndPoint.ToString() + " Login Failed!");
                }
            }
            else if (sendLoginData.Type == "Register")
            {
                FlowChatUserModel registerUserData = sendLoginData.Data as FlowChatUserModel;
                if (!userDAL.CheckUser(registerUserData.UserName, out registerUserData))
                {
                    userDAL.AddUser(registerUserData, out registerUserData);

                    FlowChatReceiveLoginDataJson recvRegData = new FlowChatReceiveLoginDataJson();
                    recvRegData.Data = registerUserData;
                    recvRegData.Type = "Register";
                    recvRegData.Status = 1;

                    loginedUserData = registerUserData;

                    tcpClient.Client.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(recvRegData)));
                }
                else
                {
                    FlowChatReceiveLoginDataJson recvRegData = new FlowChatReceiveLoginDataJson();
                    recvRegData.Data = null;
                    recvRegData.Type = "Register";
                    recvRegData.Status = 0;
                    recvRegData.Message = "The User with the same username has already existed!";

                    loginedUserData = null;

                    tcpClient.Client.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(recvRegData)));
                }
            }
        }

        private void ReturnUserList(NetworkStream stream)
        {
            FlowChatReceiveUserListDataJson recvUserListData = new FlowChatReceiveUserListDataJson();
            recvUserListData.Type = "User_List";
            recvUserListData.Status = 1;
            recvUserListData.Message = null;

            recvUserListData.UserList = getOtherUsers();

            byte[] bytes = Encoding.UTF8.GetBytes(FlowChatConsts.NETWORK_RECV_DATA_REQUEST_USER_LIST + "|" + JsonConvert.SerializeObject(recvUserListData));
            stream.Write(bytes, 0, bytes.Length);

            System.Threading.Thread.Sleep(1000);
        }

        private List<FlowChatUserModel> getOtherUsers()
        {
            List<FlowChatUserModel> usersExcludeCurrentList = new List<FlowChatUserModel>();
            foreach (var connectedClient in serverApp.ConnectedClients)
            {
                if (connectedClient.isLogined && connectedClient.loginedUserData.UserName != loginedUserData.UserName)
                {
                    usersExcludeCurrentList.Add(connectedClient.loginedUserData);
                }
            }
            return usersExcludeCurrentList;
        }

        private List<ServerAppClient> getOtherClients()
        {
            List<ServerAppClient> usersExcludeCurrentList = new List<ServerAppClient>();
            foreach (var connectedClient in serverApp.ConnectedClients)
            {
                if (connectedClient.isLogined && connectedClient.loginedUserData.UserName != loginedUserData.UserName)
                {
                    usersExcludeCurrentList.Add(connectedClient);
                }
            }
            return usersExcludeCurrentList;
        }
    }
}
