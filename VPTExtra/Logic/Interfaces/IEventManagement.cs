using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IEventManagement
    {
        public void AddEvent(Event _event);

        public void UpdateEvent(Event _event);

        public void DeleteEvent(int _eventId);
    }
}
