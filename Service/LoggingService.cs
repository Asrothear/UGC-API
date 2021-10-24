using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
/*
namespace UGC_API.Service
{
    public static class LoggingService
    {
        public static void NewLoginLog(string name, ulong socialclub, string ip, ulong hwid, bool success, string text)
        {
            using (gtaContext db = new gtaContext())
            {
                db.LogsLogin.Add(new LogsLogin
                {
                    username = name,
                    socialclub = socialclub,
                    text = text,
                    address = ip,
                    hwid = hwid,
                    success = success
                });
                db.SaveChanges();
            }
        }

        public static void NewShopLog(int charId, string charName, string type, string text,DateTime time)
        {
            try
            {
                using (gtaContext db = new gtaContext())
                {
                    db.LogsShop.Add(new LogsShop
                    {
                        charId = charId,
                        charName = charName,
                        text = text,
                        type = type,
                        timestamp = time
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void NewFactionLog(int factionId, int charId, int targetCharId, string type, string text)
        {
            try
            {
                if (factionId == 0 || charId == 0) return;
                var logData = new Logs_Faction
                {
                    factionId = factionId,
                    charId = charId,
                    targetCharId = targetCharId,
                    type = type,
                    text = text,
                    timestamp = GetTime.DateNow()
                };

                ServerFactions.LogsFaction_.Add(logData);
                
                using (gtaContext db = new gtaContext())
                {
                    db.LogsFaction.Add(logData);
                    db.SaveChanges();
                }
            }
            catch(Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void NewCompanyLog(int companyId, int charId, int targetCharId, string type, string text)
        {
            try
            {
                if (companyId <= 0 || charId <= 0) return;
                var logData = new Logs_Company
                {
                    companyId = companyId,
                    charId = charId,
                    targetCharId = targetCharId,
                    type = type,
                    text = text,
                    timestamp = GetTime.DateNow()
                };

                ServerCompanys.LogsCompany_.Add(logData);
                using (gtaContext db = new gtaContext())
                {
                    db.LogsCompany.Add(logData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void NewDeathLog(int player, int killer, uint weapon)
        {
            try
            {
                if (player <= 0 || killer <= 0) return;
                var logData = new Logs_Death
                {
                    playerName = player,
                    killerName = killer,
                    weaponId = weapon
                };

                Server_Deaths.Logs_Death_.Add(logData);
                using (gtaContext db = new gtaContext())
                {
                    db.LogsDeath.Add(logData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        public static void NewItemLog(int Player, int Target, string Item, int Amount)
        {
            try
            {
                if (Player <= 0 || Target <= 0) return;
                var logData = new Logs_Items
                {
                    player = Player,
                    target = Target,
                    item = Item,
                    amount = Amount,
                    timestamp = GetTime.DateNow()
                };

                Server_Log_Items.Logs_Items_.Add(logData);
                using (gtaContext db = new gtaContext())
                {
                    db.LogsItems.Add(logData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
*/