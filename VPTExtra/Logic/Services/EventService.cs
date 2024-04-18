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
                var events = _eventRepository.GetAllEvents();
                _logger.LogInformation("Retrieved all events successfully.");
                return events;
            }
            catch (DbException ex)
            {
                _logger.LogError("Error retrieving all events. {ErrorMessage}", ex.Message);
                throw;
            }
        }

        public Event GetEventById(int id)
        {
            try
            {
                var specificEvent = _eventRepository.GetEventById(id);
                _logger.LogInformation("Retrieved event with id {id} successfully.", id);
                return specificEvent;
            }
            catch (DbException ex)
            {
                _logger.LogError("Error retrieving event with id: {id} : {ErrorMessage}", id, ex.Message);
                throw;
            }
        }

        public bool DeleteEvent(int eventId)
        {
            try
            {
                var deleted = _eventRepository.DeleteEvent(eventId);
                _logger.LogInformation("Deleted event: {EventId}", eventId);
                return deleted;
            }
            catch (DbException ex)
            {
                _logger.LogError("Error deleting event with id: {id} : {ErrorMessage}", eventId, ex.Message);
                throw;
            }
        }
    }
}
