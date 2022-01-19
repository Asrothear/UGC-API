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
    }
}