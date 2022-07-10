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
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
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
                case "fuel":
                    CommandFunctions.Fuel(command);
                    break;
                case "range":
                    CommandFunctions.Range(command);
                    break;
                case "sortexp":
                    CommandFunctions.SortExp(command);
                    break;
                case "flush":
                    CommandFunctions.Flush(command);
                    break;
                case "carrier":
                    CommandFunctions.Carrier(command);
                    break;
                case "systems":
                    CommandFunctions.SystemsList(command);
                    break;
                case "addsystem":
                    CommandFunctions.Addsystem(command);
                    break;
                case "delsystem":
                    CommandFunctions.Delystem(command);
                    break;
                case "activity":
                    CommandFunctions.Activity(command);
                    break;
                case "updatestate":
                    CommandFunctions.UpdateState(command);
                    break;
                case "overridetick":
                    CommandFunctions.OverrideTick(command);
                    break;
            }
        }
        public static async Task Generate()
        {
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("token")
                .WithDescription("get your V2 token!")
                .AddOption("plain", ApplicationCommandOptionType.Boolean, "[Optional] Sended eine Normale Textnachricht. Für den Fall das Embeds nicht korrekt angezeigt werden", isRequired: false)
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
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("fuel")
                .WithDescription("Berechnet den ungefähren Treibstoff verbauch für die Angegebene Distanz für einen Carrier")
                .AddOption("callsign", ApplicationCommandOptionType.String, "[Optional] Rufzeichen des Carriers")
                .AddOption("distanz", ApplicationCommandOptionType.Number, "Distanz der Strecke in Lichtjahren -> 12,3", isRequired: true)
                );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("range")
                .WithDescription("Berechnet die Ungefähre Recihweite für einen Carrier")
                .AddOption("callsign", ApplicationCommandOptionType.String, "[Optional] Rufzeichen des Carriers")
                .AddOption("fuel", ApplicationCommandOptionType.Number, "[Optional] Menge des Treibstoff im Tank -> 12,3")
                .AddOption("freightfuel", ApplicationCommandOptionType.Number, "[Optional] Menge Tritium in Frachtraum -> 12,3")
                );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("sortexp")
                .WithDescription("Liest Exp-Daten aus der Log.")
                );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("flush")
                .WithDescription("Leert den Kanal.")
                );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("carrier")
                .WithDescription("Zeigt Informationen der UGC-Crrier an.")
                .AddOption("callsign", ApplicationCommandOptionType.String, "[Optional] Ruffzeichen des Carriers")
                );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("systems")
                .WithDescription("Zeigt alle Systeme an die in der BGS-Überwachung sind.")
                );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("addsystem")
                .WithDescription("Fügt ein System in die BGS-Überwachung ein.")
                .AddOption("system", ApplicationCommandOptionType.String, "Name des System", isRequired: true)
                );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("delsystem")
                .WithDescription("Entfernt ein System aus der BGS-Überwachung.")
                .AddOption("system", ApplicationCommandOptionType.String, "Name des System", isRequired: true)
                );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("activity")
                .WithDescription("Zeige die Anzahl der aktiven BGS Aktionen in den letzten x Stunden in System Y")
                .AddOption("system", ApplicationCommandOptionType.String, "Name des System", isRequired: true)
                .AddOption("stunden", ApplicationCommandOptionType.Number, "Name des System", isRequired: true)
                );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("updatestate")
                .WithDescription("Erzwingt das neuladen der Systemliste in der State-API")
                );
            _commands.Add(
                new SlashCommandBuilder()
                .WithName("overridetick")
                .WithDescription("Überschreibt den Tick")
                .AddOption("stunden", ApplicationCommandOptionType.Integer, "fügt x Stunden zum Tick hinzu.")
                .AddOption("tage", ApplicationCommandOptionType.Integer, "fügt y Tage zum Tick hinzu.")
                .AddOption("toggle", ApplicationCommandOptionType.Boolean, "True = Override AN | False = Override AUS")
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
                logger.Error(exception);
            }
        }
    }
}
