using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IEventManagement
    {
        public List<Event> GetEvents();
        public Event GetEventById(int id);
        public void CreateEvent(Event _event);

        public void UpdateEvent(Event _event);

        public void DeleteEvent(int _eventId);
    }
}
