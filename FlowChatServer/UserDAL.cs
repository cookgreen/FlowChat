using FlowChatControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatServer
{
    public class UserDAL
    {
        private SqliteHelper sqliteHelper;

        public UserDAL()
        {
            sqliteHelper = new SqliteHelper("chat_db.db");
        }

        public void AddUser(FlowChatUserModel registerUser, out FlowChatUserModel user)
        {
            sqliteHelper.ExecuteSql("insert into User(Username, Password, Avatar) values('" + registerUser.UserName + "','" + registerUser.Password + "','" + registerUser.Avatar + "')");
            CheckUser(registerUser.UserName, registerUser.Password, out user);
        }

        public bool CheckUser(string username, out FlowChatUserModel user)
        {
            user = new FlowChatUserModel();

            int count = 0;
            var reader = sqliteHelper.ExecuteSqlReader("select * from User where Username = '" + username + "'");
            while (reader.Read())
            {
                user.Uid = int.Parse(reader["id"].ToString());
                user.UserName = reader["Username"].ToString();
                user.Password = reader["Password"].ToString();
                user.Avatar = reader["Avatar"].ToString();
                count++;
            }

            reader.Close();

            return count > 0;
        }

        public bool CheckUser(string username, string password, out FlowChatUserModel user)
        {
            user = new FlowChatUserModel();

            int count = 0;
            var reader = sqliteHelper.ExecuteSqlReader("select * from User where Username = '" + username + "' and Password='" + password + "'");
            while (reader.Read())
            {
                user.Uid = int.Parse(reader["id"].ToString());
                user.UserName = reader["Username"].ToString();
                user.Password = reader["Password"].ToString();
                count++;
            }

            reader.Close();

            return count > 0;
        }
    }
}
