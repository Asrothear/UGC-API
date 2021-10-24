using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UGC_API.Functions;
using UGC_API.Handler;

namespace UGC_API.DiscordBot
{
    internal class commands
    {
        internal static void CreateToken(string[] Args)
        {
            /*if (Args.Length == 0)
            {
                //DiscordBot.DiscordLogInfo("Info", "Fehlender Parameter.", "gold");
                //return;
            }
            */
            var U_ID = CommandHandler.Message.Author.Id;
            var U_Name = CommandHandler.Message.Author.Username;
            if (!VerifyToken.ExistTokenDC(U_ID))
            {
                string NewToken = CryptHandler.HashPasword($"{U_ID}_{U_Name}");
                VerifyToken.AddToken(NewToken, U_ID, U_Name);
            }
            //DiscordBot.DiscordLogInfo("Info", $"Dein Token lautet:\n `{VerifyToken.GetToken(U_ID)}`", "gold");
            DiscordBot.SendDM("Info", $"Dein Token lautet:\n `{ VerifyToken.GetToken(U_ID)}`", "gold");
            return;
        }
    }
}
