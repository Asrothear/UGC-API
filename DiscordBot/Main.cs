using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using UGC_API.Config;
using UGC_API.Service;
using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using UGC_API.DiscordBot.Services;

namespace UGC_API.DiscordBot
{
    #region Main
    public class DiscordBot
    {
        public static DiscordSocketClient Bot;
        private InteractionService _commands;
        public static SocketMessage Smessage;
        private static bool startup = false;

        public static void Main()
            => new DiscordBot().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            if (!startup)
            {
                if (Configs.Values.Debug)
                {
                    Configs.Values.Bot.InfoChannel = Configs.Values.Bot.DevChannel;
                    Configs.Values.Bot.LogChannel = Configs.Values.Bot.DevChannel;
                }
                using (var services = ConfigureServices())
                {
                    // get the client and assign to client 
                    // you get the services via GetRequiredService<T>
                    Bot = services.GetRequiredService<DiscordSocketClient>();
                    var commands = services.GetRequiredService<InteractionService>();
                    _commands = commands;

                    // setup logging and the ready event
                    Bot.Log += LogAsync;
                    commands.Log += LogAsync;
                    Bot.Ready += ReadyAsync;

                    _ = Bot.SetActivityAsync(new Game($"auf {Configs.Values.Bot.Prefix}token", ActivityType.Watching, ActivityProperties.None));
                    // this is where we get the Token value from the configuration file, and start the bot
                    await Bot.LoginAsync(TokenType.Bot, Configs.Values.Bot.Token);
                    await Bot.StartAsync();

                    // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
                    await services.GetRequiredService<CommandHandler>().InitializeAsync();

                    await Task.Delay(Timeout.Infinite);
                }
            }
        }
        private ServiceProvider ConfigureServices()
        {
            // this returns a ServiceProvider that is used later to call for those services
            // we can add types we have access to here, hence adding the new using statement:
            // using csharpi.Services;
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
        }
        private async Task ReadyAsync()
        {
            if (!startup)
            {
                if (Configs.Values.Debug)
                {
                    //DiscordLogInfo("DEBUG", "Ready", "orange");

                    
                }
                else
                {
                    
                    DiscordLogInfo("Bot Satus", "Ready", "orange");
                }
                await _commands.RegisterCommandsToGuildAsync(Configs.Values.Bot.Guild);
                startup = true;
            }
        }
        private Task LogAsync(LogMessage msg)
        {
            Debug.WriteLine(msg.ToString());
            LoggingService.schreibeLogZeile(msg.ToString());
            return Task.CompletedTask;
        }
        #endregion
        #region Logs
        public static async void SendDM(string title, string content, string color, SocketUser user)
        {
            //if (Configs.Debug) return;
            try
            {
                var DM = user.CreateDMChannelAsync().Result;
                if (DM == null) return;
                var EmbedBuilder = new EmbedBuilder().WithColor(GetColor(color)).WithTitle(title).WithDescription(content).WithFooter(footer => footer.WithText($"© Lord Asrothear\n2020-{GetTime.DateNow().Year}")/*.WithIconUrl("https://beyondroleplay.de/media/3-logo-st-512x512-png/")*/);
                Embed embedLog = EmbedBuilder.Build();
                DM.SendMessageAsync(embed: embedLog);
            }
            catch (Exception e)
            {
                LoggingService.schreibeLogZeile(e.ToString());
                Console.WriteLine(e.ToString());
            }
        }
        public static async void DiscordLog(string title, string content, string color)
        {
            //if (Configs.Debug) return;
            try
            {
                ITextChannel textChannel = Bot.GetChannel(Configs.Values.Bot.LogChannel) as ITextChannel;
                if (textChannel == null) return;
                var EmbedBuilder = new EmbedBuilder().WithColor(GetColor(color)).WithTitle(title).WithDescription(content).WithFooter(footer => footer.WithText($"© Lord Asrothear\n2020-{GetTime.DateNow().Year}")/*.WithIconUrl("https://beyondroleplay.de/media/3-logo-st-512x512-png/")*/);
                Embed embedLog = EmbedBuilder.Build();
                textChannel.SendMessageAsync(embed: embedLog);
            }
            catch (Exception e)
            {
                LoggingService.schreibeLogZeile(e.ToString());
                Console.WriteLine(e.ToString());
            }
        }
        public static async void DiscordLogInfo(string title, string content, string color)
        {
            //if (Configs.Debug) return;
            try
            {
                ITextChannel textChannel = Bot.GetChannel(Configs.Values.Bot.InfoChannel) as ITextChannel;
                if (textChannel == null) return;
                var EmbedBuilder = new EmbedBuilder().WithColor(GetColor(color)).WithTitle(title).WithDescription(content).WithFooter(footer => footer.WithText($"© Lord Asrothear\n2020-{GetTime.DateNow().Year}")/*.WithIconUrl("https://beyondroleplay.de/media/3-logo-st-512x512-png/")*/);
                Embed embedLog = EmbedBuilder.Build();
                textChannel.SendMessageAsync(embed: embedLog);
            }
            catch (Exception e)
            {
                LoggingService.schreibeLogZeile(e.ToString());
                Console.WriteLine(e.ToString());
            }
        }

        public static Discord.Color GetColor(string col)
        {
            switch (col)
            {
                case "red": return Color.Red;
                case "green": return Color.Green;
                case "blue": return Color.Blue;
                case "oragne": return Color.Orange;
                case "yellow": return Color.Gold;
                case "gold": return Color.Gold;
            }
            return Color.Blue;
        }
        #endregion
        #region Functions
        internal static bool check_perm(SocketGuildUser user, int Need = 1)
        {
            SocketGuild guild = DiscordBot.Bot.GetGuild(Configs.Values.Bot.Guild);
            var Roles = user.Roles;
            int uLevel = 0;
            foreach (var Role in Roles)
            {
                foreach (var Perm in Configs.Values.Bot.Perms)
                {
                    if (Role.Id != Perm.Id) { continue; }
                    else
                    {
                        if (Perm.Level > uLevel) uLevel = Perm.Level;
                        if (uLevel >= Need)
                        {
                            return true;
                        }
                    }
                }
            }
            if (uLevel >= Need)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
