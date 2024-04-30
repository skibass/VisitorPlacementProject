using Interfaces.DataAcces.Repositories;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces
{
    public class EventEditRepository : IEventEditRepository
    {
        private readonly MySqlConnection db;

        public EventEditRepository(string connectionString)
        {
            db = new MySqlConnection(connectionString);
        }


    }
}
