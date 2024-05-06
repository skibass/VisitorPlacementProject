using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using Models;
using System.Net.Http.Json;

namespace API.Services
{
    public class EventApiService
    {
        private readonly HttpClient _httpClient;

        public EventApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<List<Event>> GetEvents()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Event>>("api/Event");
            return response;
        }

        public async Task<Event> GetEventById(int id)
        {
            string endpointUrl = $"api/Event/{id}";

            // Send a GET request to the API endpoint using HttpClient
            var response = await _httpClient.GetFromJsonAsync<Event>(endpointUrl);

            return response;
        }
    }
}
