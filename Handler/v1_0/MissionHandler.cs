
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UGC_API.Handler.v1_0
{
    public class MissionHandler
    {
        public static void Init()
        {/*
            switch(QLS.Event)
            {
                case "MissionAccepted":
                    MissionAccepted();
                    break;
                case "MissionCompleted":
                    MissionCompleted();
                    break;
                case "MissionAbandoned":
                    MissionAbandoned();
                    break;
                case "MissionFailed":
                    MissionFailed();
                    break;
                default:
                    return;
            }
        }
        static void MissionAccepted()
        {

        }
        static void MissionCompleted()
        {
            bool is_ugc = false;
            foreach (var Effect in QLS.json["FactionEffects"])
            {
                BGSPointsHandler.Effect = (JObject)Effect;
                DBConnect dbConnect = new DBConnect(QLS.sid);
                if (dbConnect.OpenConnection() == true)
                {

                    var system = Effect["Influence"][0]["SystemAddress"];
                    string sql = $"SELECT `name` FROM `{Configs.current.DB_prefix}*systems` WHERE adress={system}";
                    MySqlCommand cmd = new MySqlCommand(sql, dbConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            string sys_name = dataReader["name"].ToString();
                            if (Configs.current.Systems.Contains(sys_name, StringComparer.OrdinalIgnoreCase))
                            {
                                is_ugc = true;
                                BGSPointsHandler.Init();
                            }
                        }
                    }
                    dataReader.Close();
                    dbConnect.CloseConnection();
                }
            }
            if (is_ugc)
            {
                DBConnect dbConnect = new DBConnect(QLS.sid);
                if (dbConnect.OpenConnection() == true)
                {
                    string sql = $"INSERT INTO `{Configs.current.DB_prefix}*missions` (user,mission,Faction,Target,FactionEffects,timestamp)VALUES('{DBUserHandler.User.ID}','{QLS.json["Name"]}','{QLS.json["Faction"]}','{QLS.json["TargetFaction"]}','{QLS.json["FactionEffects"]}','{DateTime.Now}')";
                    MySqlCommand cmd = new MySqlCommand(sql, dbConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    dataReader.Close();
                    dbConnect.CloseConnection();
                }
            }
        }
        static void MissionAbandoned()
        {

        }
        static void MissionFailed()
        {
            */
        }
    }
}