using Interfaces.Logic;
using Interfaces.Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.Common;

namespace Logic.Services
{
    public class EventService : IEventService
    {
        private readonly ILogger<EventService> _logger;
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository, ILogger<EventService> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public List<Event> GetAllEvents()
        {
            try
            {
                return _eventRepository.GetAllEvents();
            }
            catch (DbException ex)
            {
                _logger.LogError("Error retrieving all events.", ex.GetType().FullName);

                throw;
            }
        }

        public Event GetEventById(int id)
        {
            _logger.LogInformation("Retrieving event by id: {id}.", id);
            return _eventRepository.GetEventById(id);
        }

        public void DeleteEvent(int eventId)
        {
            _logger.LogInformation("Deleted event: {EventId}", eventId);
            _eventRepository.DeleteEvent(eventId);
        }
    }
}
