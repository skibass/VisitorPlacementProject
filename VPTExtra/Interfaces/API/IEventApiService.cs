using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAcces.API
{
    public interface IEventApiService
    {
        public Task<List<Event>> GetEvents();

        public Task<Event> GetEventById(int id);
    }
}
