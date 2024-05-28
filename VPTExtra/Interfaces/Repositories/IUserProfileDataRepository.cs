using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IUserProfileDataRepository
    {
        public List<Event> RetrieveUserEvents(int userId);
        public Event RetrieveUserEventChairNames(int userId, int eventId);
    }
}
