using Discord;
using Discord.Net;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Config;
using UGC_API.DiscordBot.Modules;
using UGC_API.Service;

namespace UGC_API.DiscordBot.Services
{
    public class SlashCommands
    {
        public static List<SlashCommandBuilder> _commands = new List<SlashCommandBuilder>();
        public static List<ApplicationCommandProperties> _appCommand = new();
        internal static async Task Execute(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "token":
                    CommandFunctions.Token(command);
                    break;
                case "authcarrier":
                    CommandFunctions.Authcarrier(command);
                    break;
                case "addservice":
                    CommandFunctions.Addservice(command);
                    break;
                case "update":
                    CommandFunctions.Update(command);
                    break;
                case "findsystem":
                    CommandFunctions.Findsystem(command);
                    break;
                case "distancetohome":
                    CommandFunctions.DistanceToSol(command);
                    break;
            }
        }
        public static async Task Generate()
        {
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("token")
                .WithDescription("get your V2 token!")
                //.AddOption("user", ApplicationCommandOptionType.User, "The users whos roles you want to be listed", isRequired: true)
            );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("authcarrier")
                .WithDescription("authentificate Ownership to a Carrier")
                .AddOption("callsign", ApplicationCommandOptionType.String, "Ruffzeichen des Carriers", isRequired: true)
                .AddOption("captain", ApplicationCommandOptionType.String, "Name des Deck-Officer", isRequired: true)
            );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("addservice")
                .WithDescription("add a 3rd Party Service.")
                .AddOption("name", ApplicationCommandOptionType.String, "Name des neuen 3rd Party Service.", isRequired: true)
            );
            _commands.Add(
                 new SlashCommandBuilder()
                 .WithName("update")
                 .WithDescription("update the data cache.")
            );

            _commands.Add(
                new SlashCommandBuilder()
                .WithName("findsystem")
                .WithDescription("sucht ein System, anhand bestimmter Kriterien aus der Library raus.")
                .AddOption("minsoldistance", ApplicationCommandOptionType.Integer, "minimale Distanz zum Sol System")
                .AddOption("maxsoldistance", ApplicationCommandOptionType.Integer, "minimale Distanz zum Sol System")
                .AddOption("minpopulation", ApplicationCommandOptionType.Integer, "minimale Distanz zum Sol System")
                .AddOption("maxpopulation", ApplicationCommandOptionType.Integer, "minimale Distanz zum Sol System")
                .AddOption(
                    new SlashCommandOptionBuilder()
                    .WithName("systemallegiance")
                    .WithDescription("system Treue")
                    .WithRequired(true)
                    .AddChoice("empire", "empire")
                    .AddChoice("independent", "independent")
                    .AddChoice("federation", "federation")
                    .WithType(ApplicationCommandOptionType.String)
                )
            );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("distancetohome")
                .WithDescription("Berechnet die Entfernung des Systems zu Wapiya.")
                .AddOption("name", ApplicationCommandOptionType.String, "Name des System (System muss in der UGC-Libaray sein)", isRequired: true)
                //.AddOption("Coord", ApplicationCommandOptionType.String, "1.1,2.2,3,3")
            );
            foreach (var command in _commands)
            {
                _appCommand.Add(command.Build());
            }
        }
        public static async Task Build(IReadOnlyCollection<SocketApplicationCommand> commands, bool newBuild)
        {
            try
            {
                if (!newBuild)
                {
                    var gg = DiscordBot.Bot.GetGuild(Configs.Values.Bot.Guild);
                    await gg.BulkOverwriteApplicationCommandAsync(_appCommand.ToArray());
                }
                else {
                    foreach (var command in _appCommand)
                    {
                        await DiscordBot.Bot.Rest.CreateGuildCommand(command, Configs.Values.Bot.Guild);
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.schreibeLogZeile($"Discord_SlashComands.cs: {exception.Message}");
            }
        }
    }
}
