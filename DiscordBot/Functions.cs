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
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        internal static double GetDistance (double[] x1, double[] x2)
        {
            if(x1.Length != 3 || x2.Length != 3) { return 0; }
            double x = Math.Pow(Convert.ToDouble(x1[0], CultureInfo.InvariantCulture) - Convert.ToDouble(x2[0], CultureInfo.InvariantCulture), 2);
            double y = Math.Pow(Convert.ToDouble(x1[1], CultureInfo.InvariantCulture) - Convert.ToDouble(x2[1], CultureInfo.InvariantCulture), 2);
            double z = Math.Pow(Convert.ToDouble(x1[2], CultureInfo.InvariantCulture) - Convert.ToDouble(x2[2], CultureInfo.InvariantCulture), 2);
            double d = Math.Sqrt(x + y + z);
            d = Math.Round(d, 2);
            return d;
        }
        internal static async void AnnounceJump(Models.v1_0.CarrierModel carrier, Models.v1_0.Events.CarrierJumpRequest carrierJumpRequest, string OldSys, ulong sysa)
        {
            try
            {
                logger.Info($"AnnounceJump {carrier.Callsign}");
                ITextChannel channel = DiscordBot.Bot.GetChannel(Configs.Values.Bot.CarrierJumpChannel) as ITextChannel;
                if (channel == null) return;
                Systems.GetSystemCoords(carrierJumpRequest.SystemAddress);
                var sys = Systems._SystemData.Find(x => x.SystemAddress == carrierJumpRequest.SystemAddress);
                double[] SystemCoords = null;
                if (sys != null) SystemCoords = JsonSerializer.Deserialize<double[]>(sys.StarPos);
                if(SystemCoords == null)
                {
                    SystemCoords = await Systems.GetSystemCoordsByEddbApiAsync(carrierJumpRequest.SystemAddress);
                }

                var car = Systems._SystemData.Find(x => x.SystemAddress == sysa);
                double[] CarrierCoords = null;
                if (car != null) CarrierCoords = JsonSerializer.Deserialize<double[]>(car.StarPos);
                if (CarrierCoords == null)
                {
                    CarrierCoords = await Systems.GetSystemCoordsByEddbApiAsync(sysa);
                }
                EmbedBuilder embedb = new EmbedBuilder()
                        .WithColor(DiscordBot.GetColor("gold"))
                        .WithTitle($"Sprung Initiert - {carrier.Name}")
                        .WithDescription(carrier.Callsign)
                        .AddField("Aktuelles System", $"{OldSys}")
                        //$"<t:{dd}:F> (<t:{dd}:R>)"); long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
                        .AddField("Ziel System", $"{carrierJumpRequest.SystemName}")
                        .AddField("Sprung angefordert", $"<t:{((DateTimeOffset)GetTime.DateNow(carrierJumpRequest.timestamp)).ToUnixTimeSeconds()}:F>")
                        .AddField("Sprung beendet", $"<t:{((DateTimeOffset)GetTime.DateNow(carrierJumpRequest.timestamp.AddMinutes(15))).ToUnixTimeSeconds()}:R>")
                        .WithFooter(footer => footer.WithText(DiscordBot.foot)/*.WithIconUrl("https://beyondroleplay.de/media/3-logo-st-512x512-png/")*/);
                if (SystemCoords == null || CarrierCoords == null)
                {
                    logger.Info($"AnnounceJump {carrier.Callsign} NO CORDS");
                    embedb.AddField("\u200b", "System oder Carrier Galaxie-Koordinaten unbekannt. Treibstoff verbauch nicht berechenbar.");
                }
                else
                {
                    logger.Info($"AnnounceJump {carrier.Callsign} YES CORDS");                    
                    double d = GetDistance(CarrierCoords, SystemCoords);
                    //double distance = Math.Round(Math.Sqrt(Math.Pow(c_x - s_x, 2) + Math.Pow(c_y - s_y, 2) + Math.Pow(c_z - s_y, 2)), 2);
                    //double Fuel = Math.Round((10 + (distance / 4))*(1+((carrier.SpaceUsage.TotalCapacity-carrier.SpaceUsage.FreeSpace+carrier.FuelLevel)/25000)));
                    var f = d * (carrier.SpaceUsage.TotalCapacity - carrier.SpaceUsage.FreeSpace + carrier.FuelLevel + 25000);
                    f = f / 200000;
                    double? Fuel = Math.Round(5 + f);
                    embedb.AddField("Distanz", $"{d}ly");
                    embedb.AddField("Erwarteter Treibstoff verbrauch", $"{Fuel}t");
                }
                logger.Info($"AnnounceJump {carrier.Callsign} - {SystemCoords} - {CarrierCoords}");
                channel.SendMessageAsync(embed: embedb.Build());
            }catch(Exception ex)
            {
                logger.Error($"{ex}\nModels.v1_0.CarrierModel {carrier}, Models.v1_0.Events.CarrierJumpRequest {carrierJumpRequest}, string {OldSys}, ulong {sysa}");
                ITextChannel channel = DiscordBot.Bot.GetChannel(Configs.Values.Bot.CarrierJumpChannel) as ITextChannel;
                if (channel == null) return;
                EmbedBuilder embedb = new EmbedBuilder()
                       .WithColor(DiscordBot.GetColor("gold"))
                       .WithTitle($"Sprung Initiert - {carrier.Name}")
                       .WithDescription(carrier.Callsign)
                       .AddField("Aktuelles System", $"{OldSys}")
                       //$"<t:{dd}:F> (<t:{dd}:R>)"); long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
                       .AddField("Ziel System", $"{carrierJumpRequest.SystemName}")
                       .AddField("Sprung angefordert", $"<t:{((DateTimeOffset)GetTime.DateNow(carrierJumpRequest.timestamp)).ToUnixTimeSeconds()}:F>")
                       .AddField("Sprung beendet", $"<t:{((DateTimeOffset)GetTime.DateNow(carrierJumpRequest.timestamp.AddMinutes(15))).ToUnixTimeSeconds()}:R>")
                       .WithFooter(footer => footer.WithText(DiscordBot.foot)/*.WithIconUrl("https://beyondroleplay.de/media/3-logo-st-512x512-png/")*/);
                embedb.AddField("\u200b", "System oder Carrier Galaxie-Koordinaten unbekannt. Treibstoff verbauch nicht berechenbar. (0x2F)");
                channel.SendMessageAsync(embed: embedb.Build());
            }
        }
    }
}
