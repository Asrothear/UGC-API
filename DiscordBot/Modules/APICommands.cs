using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGC_API.DiscordBot.Services;
using UGC_API.Functions;
using UGC_API.Handler;

namespace UGC_API.DiscordBot.Modules
{
    public class APICommands : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        private CommandHandler _handler;
        public APICommands (CommandHandler handler)
        {
            _handler = handler;
        }
        #region Token gen
        [SlashCommand("token", "get your V2 token!")]
        public async Task v2token()
        {
            var U_ID = Context.User.Id;
            var U_Name = Context.User.Username;
            if (!VerifyToken.ExistTokenDC(U_ID))
            {
                string NewToken = CryptHandler.HashPasword($"{U_ID}_{U_Name}");
                VerifyToken.AddToken(NewToken, U_ID, U_Name);
            }
            DiscordBot.SendDM("Info", $"Dein Token lautet:\n `{ VerifyToken.GetToken(U_ID)}`", "gold", Context.User);
            await RespondAsync("Das Token wird dir per DM gesendet!");
        }
        #endregion
        #region authcarrier
        [SlashCommand("authcarrier", "Authentificate Ownership to a Carrier")]
        public async Task AuthCarrier(string Callsign, string Captain)
        {
            //Check Callsign
            if (Callsign.Length != 7)
            {
                RespondAsync($"Fehlerhaftes Callsign! `ABC-CDA`\n`Du hast `{Callsign}` angegeben");
                return;
            }
            var checks = Callsign.Split("-");
            if (checks.Length != 2)
            {
                RespondAsync($"Kein Callsign erkannt! Bsp. `ABC-CDA\n`Du hast `{Callsign}` angegeben");
                return;
            }
            foreach(var chk in checks)
            {
                if(chk.Length != 3)
                {
                    RespondAsync($"Callsign häkfte zu kurz! Bsp. `ABC-CDA\n`Du hast `{Callsign}` angegeben");
                    return;
                }
            }
            //Check Captain
            if(Captain.Length < 4)
            {
                RespondAsync("Fehlerhafter Cpatian Name! `John Doe`");
                return;
            }
            //Get Carrier
            var Carrier = Handler.v1_0.CarrierHandler._Carriers.FirstOrDefault(c => c.Callsign.ToLower() == Callsign.ToLower());
            if(Carrier == null)
            {
                RespondAsync("Unbekanntes Callsign!");
                return;
            }
            foreach(var Crew in Carrier.Crew)
            {
                if (Crew.CrewRole != "Captain") continue;
                if ((bool)Crew.Activated)
                {
                    if(Crew.CrewName.ToLower() == Captain.ToLower())
                    {
                        if(Carrier.OwnerDC != 0)
                        {
                            if(Carrier.OwnerDC == Context.User.Id)
                            {
                                RespondAsync("Du bist bereits diesem Carrer zugeordnet.");
                                return;
                            }
                            RespondAsync("Dieser Carrier ist bereits zugeordnet.\nWenn du glaubst das dies ein Fehler ist, bitte an Lord Asrothear wenden.");
                            return;
                        }
                        Carrier.OwnerDC = Context.User.Id;                        
                        Task.Run(() => { Handler.v1_0.CarrierHandler.UpdateCarrier(Carrier); Handler.v1_0.CarrierHandler.ParseCarrier(Carriers._Carriers); });
                        RespondAsync("Carrier Erfolgreich zugewiesen.");
                        return;
                    }
                    RespondAsync("Es konnte kein Captian in der Crew des Carriers gefunden werden.");
                }
                RespondAsync("Dieser Captain ist nicht auf dem Carrier oder aktiv.");
            }
            await RespondAsync("Da hat etwas bict gekalppt");
        }
        #endregion
        #region Add 3rd Party Service
        [SlashCommand("addservice", "Add a 3rd Party Service.")]
        public async Task addservice(string Name)
        {
            if (!DiscordBot.check_perm(Context.User as SocketGuildUser, 10))
            {
                RespondAsync("Keine Berechtigung!");
                return;
            }
            RespondAsync("Neuer Service wird redistriert!");
            Database_Models.DB_Service service = new();
            service = ServiceHandler.AddService(Name);
            DiscordBot.SendDM("Info", $"Service Registriert:\n `{service.name}`-`{service.token}`", "gold", Context.User);
        }
        #endregion
        [SlashCommand("update", "update the data cache")]
        public async Task updatechache()
        {
            await RespondAsync("Chache wir aktuallisiert.");
            TimerHandler.OnUpdateDataCacheTimer();
        }
    }
}