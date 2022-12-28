using System.Net;
using System.Net.Http.Json;

namespace PMT_Prototype.Client.Clients
{
    public class InitialStartupClient
    {
        private readonly HttpClient _client;
        public InitialStartupClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<(HttpStatusCode, List<string>?)> GetCompaniesAsync()
        {
            var response = await _client.GetAsync("companies");
            var statusCode = response.StatusCode;
            List<string>? companies = null;

            if (response.IsSuccessStatusCode)
            {
                companies = await response.Content.ReadFromJsonAsync<List<string>>();
            }

            return (statusCode, companies);
        }

        public async Task<(HttpStatusCode, List<string>?)> GetFoldersAsync()
        {
            var response = await _client.GetAsync("folders");
            var statusCode = response.StatusCode;
            List<string>? folders = null;

            if (response.IsSuccessStatusCode)
            {
                folders = await response.Content.ReadFromJsonAsync<List<string>>();
            }

            return (statusCode, folders);
        }

        public async Task<(HttpStatusCode, List<int>?)> GetTreatyYearsAsync(string company)
        {
            var response = await _client.GetAsync($"years/{company}");
            var statusCode = response.StatusCode;
            List<int>? years = null;

            if (response.IsSuccessStatusCode)
            {
                years = await response.Content.ReadFromJsonAsync<List<int>>();
            }

            return (statusCode, years);
        }

    }
}
