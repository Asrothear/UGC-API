using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace UGC_API.Handler.v1_0
{
    public class BGSPointsHandler
    {
        public static JObject Effect;
        public static void Init()
        {/*
            switch (QLS.Event)
            {
                case "RedeemVoucher":
                    RedeemVoucheer();
                    break;
                case "MultiSellExplorationData":
                case "SellExplorationData":
                    Exploration();
                    break;
                case "MarketSell":
                    MarketSell();
                    break;
                case "MissionCompleted":
                    mission();
                    break;
                default:
                    return;
            }
        }
        static async void mission()
        {   
            string f_name = Effect["Faction"].ToString();
            string trend = Effect["Influence"][0]["Trend"].ToString();
            string inf = Effect["Influence"][0]["Influence"].ToString();
            double amount = inf.Length;
            amount = 0.5 * Worker.Log2(amount);
            await Worker.SendPointsAsync(amount, f_name);
            await Worker.SendDailyAsync(inf.Length, f_name, "missions");
        }
        static async void Exploration()
        {
            if (!Configs.current.Systems.Contains(QLS.json["data_system"].ToString(), StringComparer.OrdinalIgnoreCase)) return;
            double amount = Convert.ToDouble(QLS.json["TotalEarnings"]) / 1000000;
            amount = Worker.Log2(amount);
            string faction = DBUserHandler.User.DockedFaction;
            await Worker.SendPointsAsync(amount, faction);
            await Worker.SendDailyAsync(Convert.ToDouble(QLS.json["TotalEarnings"]), faction, "explorer");
        }
        static async void MarketSell()
        {
            if (!Configs.current.Systems.Contains(QLS.json["data_system"].ToString(), StringComparer.OrdinalIgnoreCase)) return;
            double paid = Convert.ToInt32(QLS.json["AvgPricePaid"]);
            double count = Convert.ToInt32(QLS.json["Count"]);
            double sale = Convert.ToInt32(QLS.json["TotalSale"]);
            double buy = paid * count;
            double amount = sale - buy;
            amount = 0.5 * Worker.Log2(amount);
            string faction = DBUserHandler.User.DockedFaction;
            await Worker.SendPointsAsync(amount, faction);
            await Worker.SendDailyAsync(sale - buy, faction, "explorer");
        }
        static async void RedeemVoucheer()
        {
            if (QLS.json["Type"].ToString() != "bounty")return;
            if (!Configs.current.Systems.Contains(QLS.json["data_system"].ToString(), StringComparer.OrdinalIgnoreCase)) return;
            foreach (var data in QLS.json["Factions"]) {
                if (data["Faction"].ToString() == "") break;
                string faction = data["Faction"].ToString();
                double amount = (Convert.ToInt32(QLS.json["Amount"]) / 4.5) / 100000;
                amount = Math.Round(1.33 * Worker.Log2(amount), 2);
                await Worker.SendPointsAsync(amount, faction);
                await Worker.SendDailyAsync(Convert.ToDouble(QLS.json["Amount"]), faction, "voucheer");
            }
        }
    }
    static class Worker
    {
        public static string tick;
        public static async System.Threading.Tasks.Task SendPointsAsync(double amount, string faction)
        {
            await GetTickAsync();
            double last = 0;
            DBConnect dbConnect = new DBConnect(QLS.sid);
            string sql = $"SELECT * FROM {Configs.current.DB_prefix}{QLS.json["data_system"]} WHERE `Faction_Name`='{faction}' AND `Timestamp`='{tick}'";
            if (dbConnect.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(sql, dbConnect.connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    last = Convert.ToDouble(dataReader["Faction_Influence_change"]);
                }
            }
            amount += last;
            sql = $"UPDATE `{Configs.current.DB_prefix}{QLS.json["data_system"]}` SET `Faction_Influence_change`= '{amount} WHERE `Faction_Name`='{faction}' AND `Timestamp`='{tick}'";
            if (dbConnect.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(sql, dbConnect.connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Close();
                dbConnect.CloseConnection();
            }
        }
        public static async System.Threading.Tasks.Task SendDailyAsync(double amount, string faction, string type)
        {
            await GetTickAsync();
            double last = 0;
            DBConnect dbConnect = new DBConnect(QLS.sid);
            string sql = $"SELECT * FROM {Configs.current.DB_prefix}{QLS.json["data_system"]} WHERE `Faction_Name`='{faction}' AND `Timestamp`='{tick}'";
            if (dbConnect.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(sql, dbConnect.connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    last = Convert.ToDouble(dataReader[type]);
                }
            }
            amount += last;
            sql = $"UPDATE `{Configs.current.DB_prefix}{QLS.json["data_system"]}` SET `{type}`= '{amount} WHERE `Faction_Name`='{faction}' AND `Timestamp`='{tick}'";
            if (dbConnect.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(sql, dbConnect.connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Close();
                dbConnect.CloseConnection();
            }
        }
        public static double Log2(double x)
        {
            return Math.Round(Math.Log(x) / Math.Log(2), 2);
        }
        public static async System.Threading.Tasks.Task GetTickAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://elitebgs.app/api/ebgs/v5/ticks");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            responseBody = responseBody.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
            JObject json = JObject.Parse(responseBody);
            var time = json["time"];
            if (time == null)
            {
                tick = "INFO - elitebgs.app not responding";
            } 
            else
            {
                string[] list = time.ToString().Split(' ');
                string[] dates = list[0].Split('.');
                string year = dates[0];
                string mont = dates[2];
                string days = dates[1];
                tick = $"{year}-{mont}-{days}";
            }*/
        }
    }
}