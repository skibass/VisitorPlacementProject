using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IEventRepository
    {
        public List<Event> GetAllEvents();
        public Event GetEventById(int id);

        public void AddEvent(Event newEvent);
        public void UpdateEvent(Event updatedEvent);
        public void DeleteEvent(int eventId);
    }
}
