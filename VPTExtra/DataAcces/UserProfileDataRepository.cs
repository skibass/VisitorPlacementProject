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

            db.Open();

            MySqlCommand eventQ = new MySqlCommand("SELECT e.id, e.location, e.startdate, e.enddate, e.visitorlimit " +
                                       "FROM event e " +
                                       "INNER JOIN (SELECT MIN(ue.id) AS user_event_id, ue.event_id " +
                                                   "FROM user_event ue " +
                                                   "WHERE ue.user_id = @UserId " +
                                                   "GROUP BY ue.event_id) AS t ON e.id = t.event_id " +
                                       "INNER JOIN user_event ue ON t.user_event_id = ue.id", db);
            eventQ.Parameters.AddWithValue("@UserId", userId); // Assuming userId is the specified user's ID

            // Execute the query and obtain a data reader
            using (MySqlDataReader reader = eventQ.ExecuteReader())
            {
                // Check if the reader has any rows
                while (reader.Read())
                {
                    // Create a new Event object and populate its properties
                    Event newEvent = new Event
                    {
                        Id = reader.GetInt32("id"),
                        Location = reader.GetString("location"),
                        StartDate = reader.GetDateTime("startdate"),
                        EndDate = reader.GetDateTime("enddate"),
                        VisitorLimit = reader.GetInt32("visitorlimit")
                    };

                    // Add the Event object to the list
                    events.Add(newEvent);
                }
            }

            db.Close();

            return events;
        }
    }
}
