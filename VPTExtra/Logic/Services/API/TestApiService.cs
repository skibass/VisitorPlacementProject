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
    public class TestApiService
    {
        private readonly HttpClient _httpClient;

        public TestApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<List<TestWeatherForeCast>> GetWeatherForecastsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TestWeatherForeCast>>("api/WeatherForecast");
            return response;
        }

        // Add other methods for POST, PUT, DELETE operations as needed
    }
}
