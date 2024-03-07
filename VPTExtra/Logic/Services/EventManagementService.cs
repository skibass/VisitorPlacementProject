using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Models;

namespace Logic.Services
{
    public class EventManagementService : IEventManagement
    {
        private readonly IEventRepository _eventRepository;
        public EventManagementService(IEventRepository eventRepository) 
        {
            _eventRepository = eventRepository;
        }
        public List<Event> GetEvents()
        {
           return _eventRepository.GetAllEvents();
        }
        public Event GetEventById(int id)
        {
            return _eventRepository.GetEventById(id);
        }

        public void CreateEvent(Event _event)
        {
            _eventRepository.CreateEvent(_event);
        }

        public void UpdateEvent(Event _event)
        {
            _eventRepository.UpdateEvent(_event);
        }

        public void DeleteEvent(int _eventId)
        {
            _eventRepository.DeleteEvent(_eventId);
        }
    }
}
