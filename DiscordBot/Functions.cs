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
        internal static void AnnounceJump(Models.v1_0.CarrierModel carrier, Models.v1_0.Events.CarrierJumpRequest carrierJumpRequest, string OldSys)
        {
            try
            {
                LoggingService.schreibeLogZeile($"AnnounceJump {carrier.Callsign}");
                ITextChannel channel = DiscordBot.Bot.GetChannel(Configs.Values.Bot.CarrierJumpChannel) as ITextChannel;
                if (channel == null) return;
                var SystemCoords = Systems.GetSystemCoords(carrierJumpRequest.SystemAddress);
                var CarrierCoords = Systems.GetSystemCoords(carrier.SystemAdress);
                EmbedBuilder embedb = new EmbedBuilder()
                        .WithColor(DiscordBot.GetColor("gold"))
                        .WithTitle($"Sprung Initiert - {carrier.Name}")
                        .WithDescription(carrier.Callsign)
                        .AddField("Aktuelles System", $"{OldSys}")
                        //$"<t:{dd}:F> (<t:{dd}:R>)"); long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
                        .AddField("Ziel System", $"{carrierJumpRequest.SystemName}")
                        .AddField("Sprung angefordert", $"<t:{((DateTimeOffset)GetTime.DateNow(carrierJumpRequest.timestamp)).ToUnixTimeSeconds()}:F>")
                        .AddField("Sprung beendet", $"<t:{((DateTimeOffset)GetTime.DateNow(carrierJumpRequest.timestamp.AddMinutes(15))).ToUnixTimeSeconds()}:R>")
                        .WithFooter(footer => footer.WithText($"© Lord Asrothear\n2020-{GetTime.DateNow().Year}")/*.WithIconUrl("https://beyondroleplay.de/media/3-logo-st-512x512-png/")*/);
                if (SystemCoords == null || CarrierCoords == null)
                {
                    LoggingService.schreibeLogZeile($"AnnounceJump {carrier.Callsign} NO CORDS");
                    embedb.AddField("\u200b", "System oder Carrier Galaxie-Koordinaten unbekannt. Treibstoff verbauch nicht berechenbar.");
                }
                else
                {
                    LoggingService.schreibeLogZeile($"AnnounceJump {carrier.Callsign} YES CORDS");
                    double c_x = Convert.ToDouble(CarrierCoords[0], CultureInfo.InvariantCulture);
                    double c_y = Convert.ToDouble(CarrierCoords[1], CultureInfo.InvariantCulture);
                    double c_z = Convert.ToDouble(CarrierCoords[2], CultureInfo.InvariantCulture);
                    double s_x = Convert.ToDouble(SystemCoords[0], CultureInfo.InvariantCulture);
                    double s_y = Convert.ToDouble(SystemCoords[1], CultureInfo.InvariantCulture);
                    double s_z = Convert.ToDouble(SystemCoords[2], CultureInfo.InvariantCulture);
                    double distance = Math.Round(Math.Sqrt(Math.Pow(c_x - s_x, 2) + Math.Pow(c_y - s_y, 2) + Math.Pow(c_z - s_y, 2)), 2);
                    //double Fuel = Math.Round((10 + (distance / 4))*(1+((carrier.SpaceUsage.TotalCapacity-carrier.SpaceUsage.FreeSpace+carrier.FuelLevel)/25000)));
                    var x = distance * (carrier.SpaceUsage.TotalCapacity - carrier.SpaceUsage.FreeSpace + carrier.FuelLevel + 25000);
                    x = x / 200000;
                    double Fuel = Math.Round(5 + x);
                    embedb.AddField("Distanz", $"{distance}ly");
                    embedb.AddField("Erwarteter Treibstoff verbrauch", $"{Fuel}t");
                }
                LoggingService.schreibeLogZeile($"AnnounceJump {carrier.Callsign} - {SystemCoords} - {CarrierCoords}");
                channel.SendMessageAsync(embed: embedb.Build());
            }catch(Exception ex)
            {
                LoggingService.schreibeLogZeile($"{ex.ToString()}");
            }
        }
    }
}
