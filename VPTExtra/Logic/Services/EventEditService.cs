using Interfaces.DataAcces.Repositories;
using Interfaces.Logic;
using Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class EventEditService : IEventEditService
    {
        private readonly ILogger<EventEditService> _logger;
        private readonly IEventEditRepository _eventEditRepo;
        public EventEditService(IEventEditRepository eventEditRepo, ILogger<EventEditService> logger)
        {
            _eventEditRepo = eventEditRepo;
            _logger = logger;
        }
    }
}
