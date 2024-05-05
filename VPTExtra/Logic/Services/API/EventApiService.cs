using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Models;
using System.Net.Http.Json;

namespace Logic.Services.API
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
    }
}
