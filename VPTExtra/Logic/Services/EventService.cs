using Interfaces.Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class EventService
    {
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository) 
        {
            _eventRepository = eventRepository;
        }
        public List<Event> GetAllEvents()
        {          
            return _eventRepository.GetAllEvents();
        }

        public Event GetEventById(int id)
        {           
            return _eventRepository.GetEventById(id);
        }

        public void CreateEvent(Event newEvent)
        {
           _eventRepository.CreateEvent(newEvent);
        }
        public void DeleteEvent(int eventId)
        {
            _eventRepository.DeleteEvent(eventId);
        }

    }
}
