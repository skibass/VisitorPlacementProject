using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces.Interfaces
{
    public interface IEventRepository
    {
        public Event GetEventById(int eventId);
        public void AddEvent(Event newEvent);
        public void UpdateEvent(Event updatedEvent);
        public void DeleteEvent(int eventId);

    }
}
