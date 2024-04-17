using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Logic
{
    public interface IEventService
    {
        public List<Event> GetAllEvents();
        public Event GetEventById(int id);
        public void CreateEvent(Event newEvent);
        public void DeleteEvent(int eventId);
    }
}
