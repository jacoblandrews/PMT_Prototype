using System.Net.Http.Json;
using System.Net;
using PMT_Prototype.Client.DataModels;

namespace PMT_Prototype.Client.Clients
{
    public class WeatherClient
    {
        private readonly HttpClient _client;
        public WeatherClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<(HttpStatusCode, List<Properties>?)> GetWeatherAlerts()
        {
            var response = await _client.GetAsync("active?status=actual&limit=500");
            var statusCode = response.StatusCode;
            List<Properties> alerts;

            if (response.IsSuccessStatusCode)
            {
                alerts = (await response.Content.ReadFromJsonAsync<WeatherDataModel>())?.Features.Select(x => x.Properties).DistinctBy(x => x.SenderName).Take(15).ToList() ?? new List<Properties>();
            }
            else
            {
                throw new Exception($"Failed to retrieve from API. Received status code {response.StatusCode} with message {await response.Content.ReadAsStringAsync()}. ");
            }

            return (statusCode, alerts);
        }

    }
}
