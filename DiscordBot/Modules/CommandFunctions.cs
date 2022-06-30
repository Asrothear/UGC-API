using Discord;
using Discord.WebSocket;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Config;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.DiscordBot.DCModels;
using UGC_API.Functions;
using UGC_API.Handler;
using UGC_API.Handler.v1_0;
using UGC_API.Service;

namespace UGC_API.DiscordBot.Modules
{
    public class CommandFunctions
    {
        private static double[] SOL = { 0, 0, 0 };
        private static double[] Wapiya = { 54.21875, -154.84375, 30.625 };
        private static bool flushconfirm = false;
        private static bool flushtimer = false;
        private static bool flusharmed = false;
        private Task flustTask;

        private static string NumberToCurrency(long number)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            /*
            nfi.CurrencyPositivePattern = 0;  $112.24 - default
            nfi.CurrencyPositivePattern = 1;  112.24$
            nfi.CurrencyPositivePattern = 2;  $ 112.24
            nfi.CurrencyPositivePattern = 3;  112.24 $
            */
            nfi.CurrencyPositivePattern = 1;
            return string.Format(nfi, "{0:C0}", number);
        }
        private static uint StringToColor(string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);
            string hexString = BitConverter.ToString(bytes);
            hexString = hexString.Replace("-", "").Substring(1, 6);
            hexString = "0x" + hexString;
            uint col = Convert.ToUInt32(hexString, 16);
            return col;
        }
        public static async Task Token(SocketSlashCommand command)
        {
            string temp = command.Data.Options.FirstOrDefault(x => x.Name == "plain")?.Value.ToString() ?? "";
            bool mbd = false;
            if (!string.IsNullOrWhiteSpace(temp)) mbd = Convert.ToBoolean(temp);
            var U_ID = command.User.Id;
            var U_Name = command.User.Username;
            if (!VerifyToken.ExistTokenDC(U_ID))
            {
                string NewToken = CryptHandler.HashPasword($"{U_ID}_{U_Name}");
                VerifyToken.AddToken(NewToken, U_ID, U_Name);
            }
            DiscordBot.SendDM("Info", $"Dein Token lautet:\n `{ VerifyToken.GetToken(U_ID)}`", "gold", command.User, mbd);
            await command.RespondAsync("Das Token wird dir per DM gesendet!");
        }

        internal static void Activity(SocketSlashCommand command)
        {
            throw new NotImplementedException();
        }

        internal static void Delystem(SocketSlashCommand command)
        {
            string System = command.Data.Options.FirstOrDefault(x => x.Name == "system")?.Value.ToString() ?? "";
            command.RespondAsync($"Das System `{System}` wird aus der BGS-Überwachung entfernt!");
            var tmp = Configs.Systems.ToList();
            var find = tmp.Find(x => x.ToLower() == System.ToLower());
            if (find == null) return;
            tmp.Remove(System);
            tmp.Sort();
            Configs.Systems = tmp.ToArray();
            Config_F.Configs[0].systems_s = JsonSerializer.Serialize(tmp);
            using (DBContext db = new())
            {
                db.DB_Config.Update(Config_F.Configs[0]);
            }
        }

        internal static void Addsystem(SocketSlashCommand command)
        {
            string System = command.Data.Options.FirstOrDefault(x => x.Name == "system")?.Value.ToString() ?? "";
            command.RespondAsync($"Das System `{System}` wird in die BGS-Überwachung hinzugefügt!\n**!!WICHITG!!** Das System **MUSS** richtig geschrieben werden. bsp.`Korrket - >>Wapiya<< | Falsch - >>wapiya<<`");
            var tmp = Configs.Systems.ToList();
            var find = tmp.Find(x => x.ToLower() == System.ToLower());
            if (find != null) return;
            tmp.Add(System);
            tmp.Sort();
            Configs.Systems = tmp.ToArray();
            Config_F.Configs[0].systems_s = JsonSerializer.Serialize(tmp);
            using (DBContext db = new())
            {
                db.DB_Config.Update(Config_F.Configs[0]);
            }
        }

        internal static async void SystemsList(SocketSlashCommand command)
        {
            command.RespondAsync($"Liste aller {Configs.Systems.Length} Systeme wird ausgegeben.");
            string outs = "";
            foreach (var System in Configs.Systems)
            {
                string check = outs + $"{System}\n";
                if (check.Length > 200)
                {
                    outs = "";
                    await command.Channel.SendMessageAsync(check);
                    continue;
                }
                outs = outs + $"{System}\n";
            }
            if (!string.IsNullOrWhiteSpace(outs))
            {
                await command.Channel.SendMessageAsync(outs);
            }
        }

        internal static void Addservice(SocketSlashCommand command)
        {
            if (!DiscordBot.check_perm(command.User as SocketGuildUser, 10))
            {
                command.RespondAsync("Keine Berechtigung!");
                return;
            }
            command.RespondAsync("Neuer Service wird redistriert!");
            Database_Models.DB_Service service = new();
            service = ServiceHandler.AddService(command.Data.Options.FirstOrDefault(x => x.Name == "name").Value.ToString());
            DiscordBot.SendDM("Info", $"Service Registriert:\n `{service.name}`-`{service.token}`", "gold", command.User);
        }

        internal static void Carrier(SocketSlashCommand command)
        {
            string callsign = command.Data.Options.FirstOrDefault(x => x.Name == "callsign")?.Value.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(callsign))
            {
                command.RespondAsync("Liste aller Carrier wird asugegeben.");
                string outs = "";
                foreach (var carrier in CarrierHandler._Carriers)
                {
                    string check = outs + $"{carrier.Name} -`{carrier.Callsign}`\n";
                    if (check.Length > 200)
                    {
                        outs = "";
                        command.Channel.SendMessageAsync(check);
                        continue;
                    }
                    outs = outs + $"{carrier.Name} -`{carrier.Callsign}`\n";
                }
                if (!string.IsNullOrWhiteSpace(outs))
                {
                    command.Channel.SendMessageAsync(outs);
                }
                Task.Run(() =>
                {
                    Task.Delay(1500);
                    command.Channel.SendMessageAsync($"Weitere Informationen über `/carrier {{CALLSIGN}}`");
                });
                return;
            }
            var Carrier = CarrierHandler._Carriers.Find(x=> x.Callsign.ToLower() == callsign.ToLower());
            if (Carrier == null)
            {
                command.RespondAsync($"Unbekanntes Callsign! ({callsign.ToUpper()})", ephemeral:true);
                return;
            }
            command.RespondAsync($"Informationen zum Carrier `{callsign.ToUpper()}` werden ausgegeben.");
            var docked = User._Users.FindAll(x => x.docked?.ToLower() == callsign.ToLower() );
            string list = "";
            foreach(var user in docked)
            {
                if (!string.IsNullOrWhiteSpace(user.user))
                {
                    list += $"{user.user}, ";
                }else {
                    list += "Name ausgeblendet, ";
                }
            }
            
            if (string.IsNullOrWhiteSpace(list))
            {
                list = "-";
            }
            else
            {
                list = list.Remove(list.Length - 2);
            }
            var EmbedBuilder = new EmbedBuilder();
            EmbedBuilder.Title = $"{Carrier.Name}";
            EmbedBuilder.Description = $"{Carrier.Callsign}";
            EmbedBuilder.Color = new Color(StringToColor(Carrier.Callsign));
            EmbedBuilder.AddField("System", $"{Carrier.System}");
            EmbedBuilder.AddField("Angedockt", $"`{docked.Count()}`");
            EmbedBuilder.AddField("CMD´r", $"`{list}`");
            EmbedBuilder.AddField("Fuel", $"{Carrier.FuelLevel / 10}%");
            EmbedBuilder.AddField("TotalSpace", $"{Carrier.SpaceUsage.TotalCapacity}", true);
            EmbedBuilder.AddField("FreeSpace", $"{Carrier.SpaceUsage.FreeSpace}", true);
            EmbedBuilder.AddField("Cargo", $"{Carrier.SpaceUsage.Cargo}", true);
            EmbedBuilder.AddField("Crew", $"{Carrier.SpaceUsage.Crew}", true);
            EmbedBuilder.AddField("CargoReserved", $"{Carrier.SpaceUsage.CargoSpaceReserved}", true);
            EmbedBuilder.AddField("\u200B", $"\u200B", true);
            EmbedBuilder.AddField("CarrierBalance", $"{NumberToCurrency(Carrier.Finance.CarrierBalance)}", true);
            EmbedBuilder.AddField("ReserveBalance", $"{NumberToCurrency(Carrier.Finance.ReserveBalance)}", true);
            EmbedBuilder.AddField("\u200B", $"\u200B", true);
            EmbedBuilder.AddField("Access", $"{Carrier.DockingAccess}");
            EmbedBuilder.AddField("Last Update", $"{Carrier.Last_Update}");
            EmbedBuilder.WithFooter(footer => footer.WithText(DiscordBot.foot)/*.WithIconUrl("https://beyondroleplay.de/media/3-logo-st-512x512-png/")*/);
            command.Channel.SendMessageAsync(embed: EmbedBuilder.Build());
        }

        internal static async void Flush(SocketSlashCommand command)
        {

            if(!flushtimer && !flushconfirm && !flusharmed)
            {
                flushconfirm = true;
                command.RespondAsync("**!!! ACHTUNG !!! Dieser Vorgang löscht alle Nachrichten in diesem Channel!**\nUm den `flush` zu bestätigen schicke innerhalb von 10 Sekunden den Befehl erneut!", ephemeral: true);
                flushtimer = true;
                Task.Run(async () =>{
                    await Task.Delay(10 * 1000);
                    flushtimer = false;
                    if (!flusharmed) flushconfirm = false;
                });
                return;
            }
            if(flushtimer && flushconfirm && !flusharmed)
            {
                command.RespondAsync(":toilet: vorgang bestätigt.", ephemeral:true);
                flusharmed = true;
            }
            if (flusharmed && flushconfirm)
            {
                IEnumerable<IMessage> messages = new List<IMessage>();
                try
                {
                   messages = await command.Channel.GetMessagesAsync(100, mode: CacheMode.AllowDownload).FlattenAsync();
                }catch (Exception ex)
                {

                }
                foreach(IMessage message in messages)
                {
                    try
                    {
                        if(!message.IsPinned) await DiscordBot.RatelimitService.ExecuteWithRatelimits(message.Channel.DeleteMessageAsync, message.Id);
                    }catch(Exception ex) { }
                    await Task.Delay(500);
                }
                command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = $":toilet: beendet.\n Sollten noch Nachrichten übrig sein einfach nachspülen.)";
                });
                flushtimer = false;
                flushconfirm = false;
                flusharmed = false;
            }
        }

        internal static void SortExp(SocketSlashCommand command)
        {
            Task.Run(() =>
            {
                var ExplData = LogHandler._log.FindAll(x => x.Event.Contains("Sell") && x.Event.Contains("Data") && x.Timestamp > DateTime.Parse("2022 - 03 - 31 17:54:15"));
                if (ExplData == null) return;
                foreach (var item in ExplData)
                {
                    DB_Explorer newData = new();
                    newData = JsonSerializer.Deserialize<DB_Explorer>(JsonSerializer.Serialize(item));
                    newData.BaseValue = JsonSerializer.Deserialize<DB_Explorer>(item.JSON)?.BaseValue ?? 0;
                    newData.Bonus = JsonSerializer.Deserialize<DB_Explorer>(item.JSON)?.Bonus ?? 0;
                    newData.TotalEarnings = JsonSerializer.Deserialize<DB_Explorer>(item.JSON)?.TotalEarnings ?? 0;
                    newData.DataType = "Systems";
                    if (newData.BioData != null)
                    {
                        foreach (var bioData in newData.BioData)
                        {
                            newData.TotalEarnings += bioData.Value;
                        }
                        newData.DataType = "BioData";
                    }
                    try
                    {

                        using (DBContext db = new())
                        {
                            db.DB_Explorer.Add(newData);
                            db.SaveChanges();
                        }
                    }catch (Exception ex)
                    {
                        LoggingService.schreibeLogZeile(ex.Message);
                    }
                }
                command.RespondAsync("Fertig", ephemeral: true);
            });
        }

        internal static void Fuel(SocketSlashCommand command)
        {
            command.RespondAsync("Berechne...");
            string Callsign = command.Data.Options.FirstOrDefault(x => x.Name == "callsign")?.Value.ToString() ?? "";
            double distance = Convert.ToDouble(command.Data.Options.FirstOrDefault(x => x.Name == "distanz")?.Value ?? 0);
            Models.v1_0.CarrierModel carrier = new();
            if (string.IsNullOrWhiteSpace(Callsign))
            {
                carrier = CarrierHandler._Carriers.Find(x => x.OwnerDC == command.User.Id);
            }
            else
            {
                carrier = CarrierHandler._Carriers.Find(x => x.Callsign.ToLower() == Callsign.ToLower());
                if(carrier == null) carrier = CarrierHandler._Carriers.Find(x => x.OwnerDC == command.User.Id);
            }
            if (carrier == null)
            {
                command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = $"Es konnte kein Carrier gefunden werden.\n Wenn du einen Carrier besitzt stell bitte sicher das 1.Wenn angeben, das Callsign korrekt ist oder 2. du mit `/authcarrier` deinen Carrier dir zugewiesen hast!";
                });
                return;
            }
            if (distance == 0) distance = 500.00;
            EmbedBuilder embedb = new EmbedBuilder()
                .WithColor(DiscordBot.GetColor("gold"))
                .WithTitle($"Vorhersage Treibstoff verbrauch {distance}ly - {carrier.Name}")
                .WithDescription(carrier.Callsign)
                .AddField("Distanz", $"{distance}ly")
                .WithFooter(footer => footer.WithText(DiscordBot.foot)/*.WithIconUrl("https://beyondroleplay.de/media/3-logo-st-512x512-png/")*/);
            double x = 0.0;
            double y = 0.0;
            double min = 5;
            var weight = carrier.SpaceUsage.TotalCapacity - carrier.SpaceUsage.FreeSpace + carrier.FuelLevel;
            if (distance > 500)
            {
                x = 500 * (weight + 25000) / 200000;
                y = distance / 500;
                x = x * y;
                min = (min * y) + 5;
            }
            else { x = (distance * (weight + 25000)) / 200000; }
            double Fuel = Math.Round(min + x);
            embedb.AddField("Erwarteter Treibstoff verbrauch", $"{Fuel}t");
            embedb.AddField("Gesamt Tonnage", $"{weight}t");
            command.ModifyOriginalResponseAsync(x => {
                x.Content = $"";
                x.Embed = embedb.Build();
            });
        }
        internal static void Range(SocketSlashCommand command)
        {
            command.RespondAsync("Berechne...");
            string Callsign = command.Data.Options.FirstOrDefault(x => x.Name == "callsign")?.Value.ToString() ?? "";
            double fuel = Convert.ToDouble(command.Data.Options.FirstOrDefault(x => x.Name == "fuel")?.Value ?? 0);
            double freightfuel = Convert.ToDouble(command.Data.Options.FirstOrDefault(x => x.Name == "freightfuel")?.Value ?? 0);
            Models.v1_0.CarrierModel carrier = new();
            if (string.IsNullOrWhiteSpace(Callsign))
            {
                carrier = CarrierHandler._Carriers.Find(x => x.OwnerDC == command.User.Id);
            }
            else
            {
                carrier = CarrierHandler._Carriers.Find(x => x.Callsign.ToLower() == Callsign.ToLower());
                if (carrier == null) carrier = CarrierHandler._Carriers.Find(x => x.OwnerDC == command.User.Id);
            }
            if (carrier == null)
            {
                command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = $"Es konnte kein Carrier gefunden werden.\nWenn du einen Carrier besitzt stell bitte sicher das 1. Wenn angeben, das Callsign korrekt ist oder 2. du mit `/authcarrier` deinen Carrier dir zugewiesen hast!";
                });
                return;
            }
            if (fuel == 0)
            {
                fuel = carrier.FuelLevel;
            }
            EmbedBuilder embedb = new EmbedBuilder()
                .WithColor(DiscordBot.GetColor("gold"))
                .WithTitle($"Vorhersage Reichweite - {carrier.Name}")
                .WithDescription(carrier.Callsign)
                .AddField("FuelLevel", $"{fuel}t")
                .WithFooter(footer => footer.WithText(DiscordBot.foot)/*.WithIconUrl("https://beyondroleplay.de/media/3-logo-st-512x512-png/")*/);

            fuel += Math.Round(freightfuel);
            fuel -= 5;
            if(freightfuel > 0)
            {
                embedb.AddField("Treibstoff im Frachtraum", $"{freightfuel}t");
            }
            var weight = carrier.SpaceUsage.TotalCapacity - carrier.SpaceUsage.FreeSpace + carrier.FuelLevel;
            double x = fuel / ((weight + 25000) / 200000);
            double Range = Math.Round(x);
            embedb.AddField("Erwartete Reichweite" , $"{Range}ly");
            embedb.AddField("Gesamt Tonnage", $"{weight}t");
            command.ModifyOriginalResponseAsync(x => {
                x.Content = $"";
                x.Embed = embedb.Build();
            });
        }

        internal static void DistanceToSol(SocketSlashCommand command)
        {
            command.RespondAsync("Berechne...");
            string Name = command.Data.Options.FirstOrDefault(x => x.Name == "name").Value.ToString();            
            if (Name == null)
            {
                command.ModifyOriginalResponseAsync(x => {
                    x.Content = $"Argument `Name` leer.";
                });
            }
            command.ModifyOriginalResponseAsync(x => {
                x.Content = $"Berechne...{Name}";
            });
            var sys = Systems._SystemData.FindAll(x => x.StarSystem == Name);
            if(sys == null || sys.Count == 0)
            {
                command.ModifyOriginalResponseAsync(x => {
                    x.Content = $"System nicht in der UGC Library gefunden.";
                });
                return;
            }
            if (sys.Count > 1)
            {
                command.ModifyOriginalResponseAsync(x => {
                    x.Content = $"Es wurde mehr als 1 System mit diesem Namen gefunden. Ausgabe nicht möglich!";
                });
                return;
            }
            var System = sys.First();
            Systems.GetSystemCoords(System.SystemAddress);
            command.ModifyOriginalResponseAsync(x => {
                x.Content = $"{Name} ist {Functions.GetDistance(System.Coords, Wapiya)}ly von Wapiya entfernt.";
            });
        }

        internal static async void Update(SocketSlashCommand command)
        {
            await command.RespondAsync("Chache wir aktuallisiert.");
            TimerHandler.OnUpdateDataCacheTimer();
        }

        internal static void Findsystem(SocketSlashCommand command)
        {
            command.RespondAsync("Suche läuft", ephemeral: true);
            List<SystemData> stack = new List<SystemData>();
            int minsoldistance = Convert.ToInt32(command.Data.Options.FirstOrDefault(x => x.Name == "minsoldistance")?.Value);
            int maxsoldistance = Convert.ToInt32(command.Data.Options.FirstOrDefault(x => x.Name == "maxsoldistance")?.Value);
            long minpopulation = Convert.ToInt64(command.Data.Options.FirstOrDefault(x => x.Name == "minpopulation")?.Value);
            long maxpopulation = Convert.ToInt64(command.Data.Options.FirstOrDefault(x => x.Name == "maxpopulation")?.Value);
            string systemallegiance = command.Data.Options.FirstOrDefault(x => x.Name == "systemallegiance").Value.ToString();
            List<DB_SystemData> haystack = new List<DB_SystemData>(Systems._SystemData);
            if (haystack.Count == 0)
            {
                command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = $"{haystack.Count} Systeme gefunden.";
                });
                return;
            };
            haystack.Remove(haystack.Find(x => x.SystemAddress == 10477373803));
            List<DB_SystemData> needle = new List<DB_SystemData>();
            needle = haystack.FindAll(x => x.SystemAllegiance?.ToLower() == systemallegiance.ToLower());
            //needle = needle.FindAll(x => x.Population > 500);
            if (minpopulation > 0)
            {
                needle = needle.FindAll(x => x.Population < minpopulation);
            }
            if (maxpopulation > 0 && maxpopulation < 500000000)
            {
                needle = needle.FindAll(x => x.Population > maxpopulation);
            }
            foreach (var sysd in needle)
            {
                Systems.GetSystemCoords(sysd.SystemAddress);
            }
            if (minsoldistance > 1)
            {
                needle = needle.FindAll(x => Functions.GetDistance(x.Coords, SOL) > minsoldistance);
            }
            if (maxsoldistance > 0 && maxsoldistance < 501)
            {
                needle = needle.FindAll(x => Functions.GetDistance(x.Coords, SOL) < maxsoldistance);
            }
            if(needle.Count == 0)
            {
                command.ModifyOriginalResponseAsync(x => {
                    x.Content = $"{needle.Count} Systeme gefunden.";
                });
                return;
            }
            foreach(var stich in needle)
            {
                var straw = JsonSerializer.Deserialize<SystemData>(JsonSerializer.Serialize(stich));
                straw.DistanceToSol = Functions.GetDistance(straw.Coords, SOL);
                straw.Factions = JsonSerializer.Deserialize<List<SystemData.FactionsModel>>(stich.Faction_String);
                stack.Add(straw);
            }            
            using (var tempFiles = new TempFileCollection())
            {
                string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tempDirectory);
                string file = tempFiles.AddExtension("json");
                File.WriteAllText(file, JsonSerializer.Serialize(stack, new JsonSerializerOptions { WriteIndented = true }).Replace(@"\u0022", "\"").Replace(@"\u0027", "'"));
                File.Copy(file, Path.Combine(tempDirectory, Path.GetFileName(file)));
                var zn = $"{Path.GetTempPath()}{Path.GetFileName(file)}.zip";
                ZipFile.CreateFromDirectory(tempDirectory, zn);
                List<FileAttachment> files = new List<FileAttachment>();
                files.Add(new FileAttachment(zn));
                command.ModifyOriginalResponseAsync(x=> {
                    x.Attachments = files;
                    x.Content = $"{stack.Count} Systeme gefunden. Sende Datei.";
                    });
                File.Delete(zn);
                Directory.Delete(tempDirectory, true);
                //Directory.Delete(zn.Replace(Path.GetFileName(file),$"Temp1_{Path.GetFileName(file)}"), true);
                tempFiles.Delete();
            }
        }

        internal static void Authcarrier(SocketSlashCommand command)
        {
            //Check Callsign
            var Callsign = command.Data.Options.FirstOrDefault(x => x.Name == "callsign").Value.ToString();
            var Captain = command.Data.Options.FirstOrDefault(x => x.Name == "captain").Value.ToString();
            if (Callsign.Length != 7)
            {
                command.RespondAsync($"Fehlerhaftes Callsign! `ABC-CDA`\n`Du hast `{Callsign}` angegeben");
                return;
            }
            var checks = Callsign.Split("-");
            if (checks.Length != 2)
            {
                command.RespondAsync($"Kein Callsign erkannt! Bsp. `ABC-CDA\n`Du hast `{Callsign}` angegeben");
                return;
            }
            foreach (var chk in checks)
            {
                if (chk.Length != 3)
                {
                    command.RespondAsync($"Callsign hälfte zu kurz! Bsp. `ABC-CDA\n`Du hast `{Callsign}` angegeben");
                    return;
                }
            }
            //Check Captain
            if (Captain.Length < 4)
            {
                command.RespondAsync("Fehlerhafter Cpatian Name! `John Doe`");
                return;
            }
            //Get Carrier
            var Carrier = Handler.v1_0.CarrierHandler._Carriers.FirstOrDefault(c => c.Callsign.ToLower() == Callsign.ToLower());
            if (Carrier == null)
            {
                command.RespondAsync("Unbekanntes Callsign!");
                return;
            }
            foreach (var Crew in Carrier.Crew)
            {
                if (Crew.CrewRole != "Captain") continue;
                if ((bool)Crew.Activated)
                {
                    if (Crew.CrewName.ToLower() == Captain.ToLower())
                    {
                        if (Carrier.OwnerDC != 0)
                        {
                            if (Carrier.OwnerDC == command.User.Id)
                            {
                                command.RespondAsync("Du bist bereits diesem Carrer zugeordnet.");
                                return;
                            }
                            command.RespondAsync("Dieser Carrier ist bereits zugeordnet.\nWenn du glaubst das dies ein Fehler ist, bitte an Lord Asrothear wenden.");
                            return;
                        }
                        Carrier.OwnerDC = command.User.Id;
                        Task.Run(() => { Handler.v1_0.CarrierHandler.UpdateCarrier(Carrier); Handler.v1_0.CarrierHandler.ParseCarrier(Carriers._Carriers); });
                        command.RespondAsync("Carrier Erfolgreich zugewiesen.");
                        return;
                    }
                    command.RespondAsync("Es konnte kein Captian in der Crew des Carriers gefunden werden.");
                }
                command.RespondAsync("Dieser Captain ist nicht auf dem Carrier oder aktiv.");
            }
            command.RespondAsync("Da hat etwas nicht gekalppt");
        }
    }
}
