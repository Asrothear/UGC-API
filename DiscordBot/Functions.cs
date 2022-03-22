using Discord;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Config;
using UGC_API.Functions;
using UGC_API.Service;

namespace UGC_API.DiscordBot
{
    public class Functions
    {
        internal static void AnnounceJump(Models.v1_0.CarrierModel carrier, Models.v1_0.Events.CarrierJumpRequest carrierJumpRequest)
        {
            var channel = DiscordBot.Bot.GetChannel(Configs.Values.Bot.CarrierJumpChannel) as ITextChannel;
            var SystemCoords = Systems.GetSystemCoords(carrierJumpRequest.SystemAddress);
            var CarrierCoords = Systems.GetSystemCoords(carrier.SystemAdress);
            EmbedBuilder embedb = new EmbedBuilder()
                    .WithColor(DiscordBot.GetColor("gold"))
                    .WithTitle($"Sprung Initiert - {carrier.Name}")
                    .WithDescription(carrier.Callsign)
                    .AddField("Aktuelles System", $"{carrier.prev_System}")
                    //$"<t:{dd}:F> (<t:{dd}:R>)");
                    .WithFooter(footer => footer.WithText($"© Lord Asrothear\n2020-{GetTime.DateNow().Year}")/*.WithIconUrl("https://beyondroleplay.de/media/3-logo-st-512x512-png/")*/);
            if (SystemCoords == null || CarrierCoords == null)
            {
                embedb
                    .AddField("Ziel System", $"{carrierJumpRequest.SystemName}")
                    .AddField("Sprung angefordert", $"<t:{GetTime.DateNow(carrierJumpRequest.timestamp)}:F>")
                    .AddField("Sprung beendet", $"<t:{GetTime.DateNow(carrierJumpRequest.timestamp.AddMinutes(15))}:R>")
                    .AddField("\u200b", "System oder Carrier Galaxie-Koordinaten unbekannt. Treibstoff verbauch nicht berechenbar.");
            }
            else
            {
                double distance = Math.Round(Math.Sqrt(Math.Pow(carrier.last_pos[0] - SystemCoords[0], 2) + Math.Pow(carrier.last_pos[1] - SystemCoords[1], 2) + Math.Pow(carrier.last_pos[2] - SystemCoords[2], 2)), 2);
                double Fuel = Math.Round((10 + (distance / 4))*(1+((carrier.SpaceUsage.TotalCapacity-carrier.SpaceUsage.FreeSpace+carrier.FuelLevel)/25000)));
                embedb.AddField("Erwarteter Treibstoff verbrauch", $"{Fuel}");
            }
            channel.SendMessageAsync(embed: embedb.Build());
        }
    }
}
