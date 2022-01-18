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
            if (!startup )
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
                    await _commands.RegisterCommandsToGuildAsync(Configs.Values.Bot.Guild);
                    DiscordLogInfo("Bot Satus", "Ready", "orange");
                }
                startup = true;
            }
        }
        private Task LogAsync(LogMessage msg)
        {
            Debug.WriteLine(msg.ToString());
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
                Console.WriteLine(e.ToString());
            }
        }

        public static Discord.Color GetColor(string col)
        {
            switch(col)
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
        #region handle
        private static async Task Hanndle(SocketMessage message)
        {
            Smessage = message;
            await Task.Run(Command);
            return;
        }
        public static void Command ()
        {
            SocketMessage message = Smessage;
            string command = "";
            int lengthOfCommand = -1;
            int lengthOfPrefix = -1;
            lengthOfPrefix = Configs.Values.Bot.Prefix.Length;
            if (!message.Content.StartsWith(Configs.Values.Bot.Prefix)) return;
            if (message.Author.IsBot) return;
            if (!Configs.Values.Debug && message.Channel.Id != Configs.Values.Bot.InfoChannel) return;
            if (Configs.Values.Debug && message.Channel.Id != Configs.Values.Bot.DevChannel) return;
            lengthOfCommand = message.Content.Length;

            command = message.Content.Substring(lengthOfPrefix, lengthOfCommand - lengthOfPrefix);

            //CommandHandler.Execute(command, message);
            return;
        }
        #endregion
    }
}
