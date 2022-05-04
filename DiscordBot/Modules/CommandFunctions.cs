using Discord;
using Discord.WebSocket;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Database_Models;
using UGC_API.DiscordBot.DCModels;
using UGC_API.Functions;
using UGC_API.Handler;

namespace UGC_API.DiscordBot.Modules
{
    public class CommandFunctions
    {
        public static async Task Token(SocketSlashCommand command)
        {
            var U_ID = command.User.Id;
            var U_Name = command.User.Username;
            if (!VerifyToken.ExistTokenDC(U_ID))
            {
                string NewToken = CryptHandler.HashPasword($"{U_ID}_{U_Name}");
                VerifyToken.AddToken(NewToken, U_ID, U_Name);
            }
            DiscordBot.SendDM("Info", $"Dein Token lautet:\n `{ VerifyToken.GetToken(U_ID)}`", "gold", command.User);
            await command.RespondAsync("Das Token wird dir per DM gesendet!");
        }

        internal static async void Addservice(SocketSlashCommand command)
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

        internal static async void Update(SocketSlashCommand command)
        {
            await command.RespondAsync("Chache wir aktuallisiert.");
            TimerHandler.OnUpdateDataCacheTimer();
        }

        internal async static void Findsystem(SocketSlashCommand command)
        {
            await command.RespondAsync("Suche läuft", ephemeral: true);
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
            double[] SOL = { 0, 0, 0 };
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
                await command.ModifyOriginalResponseAsync(x=> {
                    x.Attachments = files;
                    x.Content = $"{stack.Count} Systeme gefunden. Sende Datei.";
                    });
                File.Delete(zn);
                Directory.Delete(tempDirectory, true);
                //Directory.Delete(zn.Replace(Path.GetFileName(file),$"Temp1_{Path.GetFileName(file)}"), true);
                tempFiles.Delete();
            }
        }

        internal static async void Authcarrier(SocketSlashCommand command)
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
            await command.RespondAsync("Da hat etwas nicht gekalppt");
        }
    }
}
