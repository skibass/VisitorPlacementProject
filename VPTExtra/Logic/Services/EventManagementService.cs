using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcces.Interfaces;
using Logic.Interfaces;
using Models;

namespace Logic.Services
{
    public class EventManagementService : IEventManagement
    {
        IEventRepository _eventRepository;
        public EventManagementService(IEventRepository eventRepository) 
        {
            _eventRepository = eventRepository;
        }
        public void AddEvent(Event _event)
        {
            _eventRepository.AddEvent(_event);
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
