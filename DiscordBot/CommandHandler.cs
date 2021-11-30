using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
using Discord.WebSocket;
using UGC_API.Config;
using UGC_API.Handler.v1_0;

namespace UGC_API.DiscordBot

{
    class CommandHandler
    {
        internal static string[] Args;
        internal static SocketMessage Message;
        public static void Execute(string command, SocketMessage message)
        {
            Message = message;
            command = command.ToLower();
            if (command.Contains(' '))
            {
                Args = command.Split(' ');
                command = Args[0];
                Args = Args.Skip(1).ToArray();
            }
            switch (command)
            {
                case "token":
                    commands.CreateToken(Args);
                    break;
                default:
                    DiscordBot.DiscordLogInfo("Info","Befehl nicht erkannt.","gold");
                    break;
            }
        }
        internal static bool check_perm()
        {
            SocketGuild guild = DiscordBot.Bot.GetGuild(Configs.Values.Bot.Guild);
            ulong AuthorId = Message.Author.Id;
            var user = Message.Author as SocketGuildUser;
            var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Role");
            if (!user.Roles.Contains(role))
            {
                // Do Stuff
                if (user.GuildPermissions.KickMembers)
                {
                    //user.KickAsync();
                }
                return true;
            }
            return true;
        }
    }
}
