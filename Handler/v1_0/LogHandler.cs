using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UGC_WebAPI_v2.Controllers.qls
{
    public class LogHandler
    {
        public static void Log()
        {/*
            string target;
            if (DBUserHandler.User.Name != "EDDB")
            {
                target = "*log";
            }
            else
            {
                target = "*eddn_log";
                return;
            }
            DBConnect dbConnect = new DBConnect(QLS.sid);
            UserData User = DBUserHandler.User;
            TimeSpan ntime = QLS.stopwatch.Elapsed;
            if (dbConnect.OpenConnection() == true)
            {
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ntime.Hours, ntime.Minutes, ntime.Seconds, ntime.Milliseconds / 10);
                string sql = $"INSERT INTO `{Configs.current.DB_prefix}{target}` (`Timestamp`,`User`,`Event`,`ntime`,`ninsert`,`JSON`,`version_plugin`) VALUES('{User.LastDataInsert}','{User.ID}','{QLS.Event}','{User.LastDataInsert}','{elapsedTime}','{JsonConvert.SerializeObject(QLS.json)}','{User.PluginVersion + "." + User.Branch}')";
                MySqlCommand cmd = new MySqlCommand(sql, dbConnect.connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Close();
                dbConnect.CloseConnection();
            }*/
        }
    }
}