using System.Net.Http;
using System.Text.Json;

namespace UGC_API.Service
{
    public class BGSOrderAPI
    {
        internal static Models.Service.BGSOrderApiModel ActualBGSOrder = new();
        internal static async void GetCurrentList()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using HttpClient Client = new HttpClient(handler);
            var response = await Client.GetAsync("https://anweisungen.ugc-bgs.de:3000/api/bgs_order");
            var json = await response.Content.ReadAsStringAsync();
            ActualBGSOrder = JsonSerializer.Deserialize<Models.Service.BGSOrderApiModel>(json);
        }
    }
}