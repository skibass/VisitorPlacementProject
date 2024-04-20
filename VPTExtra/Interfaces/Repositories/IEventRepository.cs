using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IEventRepository
    {
        public List<Event> GetAllEvents();
        public Event GetEventById(int id);
        public bool CreateEvent(Event newEvent);
        public void UpdateEvent(Event updatedEvent);
        public bool DeleteEvent(int eventId);
    }
}
