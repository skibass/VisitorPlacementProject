using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VPT_user_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private List<WeatherForecast> _weatherForecasts; // Sample data store (you can use a database)

        public WeatherForecastController(ILogger<WeatherForecastController> logger, List<WeatherForecast> weatherForecasts)
        {
            _logger = logger;
            _weatherForecasts = weatherForecasts; // Initialize the list
        }      

        [HttpPost]
        public IActionResult Create([FromBody] WeatherForecast newForecast)
        {
            if (newForecast == null)
            {
                return BadRequest("Invalid weather forecast data");
            }

            // Generate a unique ID for the new forecast
            int nextId = _weatherForecasts.Count > 0 ? _weatherForecasts.Max(f => f.Id) + 1 : 1;
            newForecast.Id = nextId;

            // Add the new forecast to the list
            _weatherForecasts.Add(newForecast);

            // Return HTTP 201 Created with the newly created forecast
            return CreatedAtAction(nameof(GetById), new { id = newForecast.Id }, newForecast);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] WeatherForecast updatedForecast)
        {
            var existingForecast = _weatherForecasts.FirstOrDefault(f => f.Id == id);
            if (existingForecast == null)
            {
                return NotFound();
            }

            // Update existing forecast properties
            existingForecast.Date = updatedForecast.Date;
            existingForecast.TemperatureC = updatedForecast.TemperatureC;
            existingForecast.Summary = updatedForecast.Summary;

            // Return HTTP 200 OK with the updated forecast
            return Ok(existingForecast);
        }

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation("Attempting to retrieve forecast with ID: {id}", id);

            var forecast = _weatherForecasts.FirstOrDefault(f => f.Id == id);
            if (forecast == null)
            {
                _logger.LogInformation("Forecast with ID {id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Forecast found: {@forecast}", forecast);
            return Ok(forecast);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return _weatherForecasts;
        }
    }

    public class WeatherForecast
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}