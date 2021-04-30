using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace FlowChatServer
{
    public class SqliteHelper
    {
        private string db;

        public SqliteHelper(string db)
        {
            this.db = db;
        }

        private SQLiteConnection createConnection()
        {
            var connection = new SQLiteConnection("Data Source=" + db + "");
            connection.Open();
            return connection;
        }

        public void ExecuteSql(string sql)
        {
            var conn = createConnection();
            try
            {
                SQLiteCommand command = new SQLiteCommand();
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

        public SQLiteDataReader ExecuteSqlReader(string sql)
        {
            var conn = createConnection();
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = sql;
            command.Connection = conn;
            var reader = command.ExecuteReader();
            return reader;
        }
    }
}
