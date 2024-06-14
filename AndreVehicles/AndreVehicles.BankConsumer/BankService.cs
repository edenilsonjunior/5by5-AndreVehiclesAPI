using Models.Financials;
using Newtonsoft.Json;
using System.Text;

namespace AndreVehicles.BankConsumer
{
    public class BankService
    {
        private static readonly HttpClient _httpClient = new HttpClient();



        public async Task<Bank> Post(string uri, Bank bank)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(bank), Encoding.UTF8, "application/json");
                HttpResponseMessage respose = await _httpClient.PostAsync(uri, content);
                respose.EnsureSuccessStatusCode();
                string bankReturn = await respose.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Bank>(bankReturn);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
