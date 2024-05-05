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
        private List<Event> _events;
        private EventRepo _eventRepo;

        public EventController(ILogger<EventController> logger, EventRepo eventRepo)
        {
            _logger = logger;
            _events = new List<Event>();
            _eventRepo = eventRepo;
        }

        [HttpGet(Name = "GetEvents")]
        public IEnumerable<Event> Get()
        {
            _events = _eventRepo.GetAllEvents();

            return _events;
        }
    }
}