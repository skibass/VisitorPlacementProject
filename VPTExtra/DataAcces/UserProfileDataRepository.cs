using Interfaces.Repositories;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAcces
{
    public class UserProfileDataRepository : IUserProfileDataRepository
    {
        private readonly MySqlConnection db;

        public UserProfileDataRepository(string connectionString)
        {
            db = new MySqlConnection(connectionString);
        }

        public List<Event> RetrieveUserEvents(int userId)
        {
            List<Event> events = new List<Event>();

            try
            {
                db.Open();

                MySqlCommand eventQ = new MySqlCommand(
    @"SELECT e.id, e.location, e.startdate, e.enddate, e.visitorlimit, 
  (SELECT COUNT(*) FROM user_event WHERE user_id = @UserId AND event_id = e.id) AS chairs_reserved,
  GROUP_CONCAT(c.name SEPARATOR ', ') AS chair_names
FROM event e 
INNER JOIN user_event ue ON e.id = ue.event_id
INNER JOIN chair c ON ue.chair_id = c.id
WHERE ue.user_id = @UserId
GROUP BY e.id", db);

                eventQ.Parameters.AddWithValue("@UserId", userId);

                using (MySqlDataReader reader = eventQ.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Event newEvent = new Event
                        {
                            Id = reader.GetInt32("id"),
                            Location = reader.GetString("location"),
                            StartDate = reader.GetDateTime("startdate"),
                            EndDate = reader.GetDateTime("enddate"),
                            VisitorLimit = reader.GetInt32("visitorlimit"),
                            ChairsReserved = reader.GetInt32("chairs_reserved"),
                            ChairNames = reader.GetString("chair_names")
                        };

                        events.Add(newEvent);
                    }
                }
            }
            finally
            {
                db.Close();
            }

            return events;
        }

        public Event RetrieveUserEventChairNames(int userId, int eventId)
        {
            Event _event = new Event();

            try
            {
                db.Open();

                string query = @"
        SELECT e.id, e.location,
               GROUP_CONCAT(c.name SEPARATOR ', ') AS chair_names
        FROM event e 
        INNER JOIN user_event ue ON e.id = ue.event_id
        INNER JOIN chair c ON ue.chair_id = c.id
        WHERE ue.user_id = @UserId AND e.id = @EventId
        GROUP BY e.id, e.location";

                MySqlCommand eventQ = new MySqlCommand(query, db);

                eventQ.Parameters.AddWithValue("@UserId", userId);
                eventQ.Parameters.AddWithValue("@EventId", eventId);

                using (MySqlDataReader reader = eventQ.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Event newEvent = new Event
                        {
                            Id = reader.GetInt32("id"),
                            Location = reader.GetString("location"),
                            ChairNames = reader.GetString("chair_names")
                        };

                        _event = newEvent;
                    }
                }
            }
            finally
            {
                db.Close();
            }

            return _event;
        }
    }
}
