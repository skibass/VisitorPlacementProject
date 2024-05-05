using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using VPT_user_api.Repos;
using VPT_user_api.Models;

namespace VPT_user_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private List<Event> _events; // Sample data store (you can use a database)
        private EventRepo _eventRepo; // Sample data store (you can use a database)

        public EventController(ILogger<EventController> logger, EventRepo eventRepo)
        {
            _logger = logger;
            _events = new List<Event>(); // Initialize the list
            _eventRepo = eventRepo;
        }

        //[HttpPost]
        //public IActionResult Create([FromBody] WeatherForecast newForecast)
        //{
        //    if (newForecast == null)
        //    {
        //        return BadRequest("Invalid weather forecast data");
        //    }

        //    // Generate a unique ID for the new forecast
        //    int nextId = _weatherForecasts.Count > 0 ? _weatherForecasts.Max(f => f.Id) + 1 : 1;
        //    newForecast.Id = nextId;

        //    // Add the new forecast to the list
        //    _weatherForecasts.Add(newForecast);

        //    // Return HTTP 201 Created with the newly created forecast
        //    return CreatedAtAction(nameof(GetById), new { id = newForecast.Id }, newForecast);
        //}

        //[HttpPut("{id}")]
        //public IActionResult Update(int id, [FromBody] WeatherForecast updatedForecast)
        //{
        //    var existingForecast = _weatherForecasts.FirstOrDefault(f => f.Id == id);
        //    if (existingForecast == null)
        //    {
        //        return NotFound();
        //    }

        //    // Update existing forecast properties
        //    existingForecast.Date = updatedForecast.Date;
        //    existingForecast.TemperatureC = updatedForecast.TemperatureC;
        //    existingForecast.Summary = updatedForecast.Summary;

        //    // Return HTTP 200 OK with the updated forecast
        //    return Ok(existingForecast);
        //}

        //[HttpGet("{id}", Name = "GetById")]
        //public IActionResult GetById(int id)
        //{
        //    _logger.LogInformation("Attempting to retrieve forecast with ID: {id}", id);

        //    var forecast = _weatherForecasts.FirstOrDefault(f => f.Id == id);
        //    if (forecast == null)
        //    {
        //        _logger.LogInformation("Forecast with ID {id} not found", id);
        //        return NotFound();
        //    }

        //    _logger.LogInformation("Forecast found: {@forecast}", forecast);
        //    return Ok(forecast);
        //}

        [HttpGet(Name = "GetEvents")]
        public IEnumerable<Event> Get()
        {
            _events = _eventRepo.GetAllEvents();

            return _events;
        }
    }
}