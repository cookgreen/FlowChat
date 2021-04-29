using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatServer
{
    public class SqliteHelper
    {
        private string db;

        public SqliteHelper(string db)
        {
            this.db = db;
        }

        private SqliteConnection createConnection()
        {
            var connection = new SqliteConnection("Data Source=" + db + "");
            connection.Open();
            return connection;
        }

        public void ExecuteSql(string sql)
        {
            var conn = createConnection();
            try
            {
                SqliteCommand command = new SqliteCommand();
                command.CommandText = sql;
                command.Connection = conn;
                command.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                conn.Close();
            }
        }

        public SqliteDataReader ExecuteSqlReader(string sql)
        {
            var conn = createConnection();
            SqliteCommand command = new SqliteCommand();
            command.CommandText = sql;
            command.Connection = conn;
            var reader = command.ExecuteReader();
            conn.Close();
            return reader;
        }
    }
}
