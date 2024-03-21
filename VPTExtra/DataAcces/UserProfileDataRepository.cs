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
                #region Chatgpt help
                    // Chat gpt: i want to combine these 2 queries

                    // MySqlCommand eventQ = new MySqlCommand("SELECT COUNT(*) " +
                    //                  "FROM user_event " +
                    //                  "WHERE user_id = @UserId AND event_id = @EventId", db);

                    // MySqlCommand eventQ = new MySqlCommand("SELECT e.id, e.location, e.startdate, e.enddate, e.visitorlimit " +
                    //                           "FROM event e " +
                    //                           "INNER JOIN (SELECT MIN(ue.id) AS user_event_id, ue.event_id " +
                    //                                       "FROM user_event ue " +
                    //                                       "WHERE ue.user_id = @UserId " +
                    //                                       "GROUP BY ue.event_id) AS t ON e.id = t.event_id " +
                    //                           "INNER JOIN user_event ue ON t.user_event_id = ue.id", db);

                    // Result:
                #endregion
                    "SELECT e.id, e.location, e.startdate, e.enddate, e.visitorlimit, " +
                    "(SELECT COUNT(*) FROM user_event WHERE user_id = @UserId AND event_id = e.id) AS chairs_reserved " +
                    "FROM event e " +
                    "INNER JOIN (SELECT MIN(ue.id) AS user_event_id, ue.event_id " +
                                "FROM user_event ue " +
                                "WHERE ue.user_id = @UserId " +
                                "GROUP BY ue.event_id) AS t ON e.id = t.event_id " +
                    "INNER JOIN user_event ue ON t.user_event_id = ue.id", db);

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
                            ChairsReserved = reader.GetInt32("chairs_reserved")
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
    }
}
