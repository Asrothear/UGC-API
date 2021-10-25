using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using UGC_API.Config;
using UGC_API.Service;
using System.Diagnostics;

namespace UGC_API.DiscordBot
{
    #region Main
    public class DiscordBot
    {
        public static DiscordSocketClient Bot;
        public static SocketMessage Smessage;
        private static bool startup = false;

        public static void Main()
            => new DiscordBot().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            if (!startup )
            {
                if (Configs.Debug)
                {
                    BotConfiguration.discordBotInfoChannel = BotConfiguration.discordBotDevChannel;
                    BotConfiguration.discordBotLogChannel = BotConfiguration.discordBotDevChannel;
                }
                Bot = new DiscordSocketClient();
                Bot.MessageReceived += Hanndle;
                Bot.Ready += Ready;
                Bot.Log += Log;
                var token = BotConfiguration.discordBotToken;
                await Bot.LoginAsync(TokenType.Bot, token);
                await Bot.StartAsync();
            }
        }
        private static Task Ready()
        {
            if (!startup)
            {
                if (Configs.Debug)
                {
                    //DiscordLogInfo("DEBUG", "Ready", "orange");
                }
                else
                {
                    DiscordLogInfo("Bot Satus", "Ready", "orange");
                }
                startup = true;
            }
            return Task.CompletedTask;
        }
        private static Task Log(LogMessage msg)
        {
            Debug.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        #endregion
        #region Logs
        public static async void SendDM(string title, string content, string color)
        {
            //if (Configs.Debug) return;
            try
            {
                var DM = CommandHandler.Message.Author.GetOrCreateDMChannelAsync().Result;
                ITextChannel textChannel = Bot.GetChannel(BotConfiguration.discordBotLogChannel) as ITextChannel;
                if (textChannel == null) return;
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
                ITextChannel textChannel = Bot.GetChannel(BotConfiguration.discordBotLogChannel) as ITextChannel;
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
                ITextChannel textChannel = Bot.GetChannel(BotConfiguration.discordBotInfoChannel) as ITextChannel;
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
            lengthOfPrefix = BotConfiguration.prefix.Length;
            if (!message.Content.StartsWith(BotConfiguration.prefix)) return;
            if (message.Author.IsBot) return;
            if (!Configs.Debug && message.Channel.Id == 840506667837554698) return;
            //if (Configs.Debug && message.Channel.Id != 739971744399622215) return;
            lengthOfCommand = message.Content.Length;

            command = message.Content.Substring(lengthOfPrefix, lengthOfCommand - lengthOfPrefix);

            CommandHandler.Execute(command, message);
            return;
        }
        #endregion
    }
}
