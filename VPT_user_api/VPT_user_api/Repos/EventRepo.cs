using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using VPT_user_api.Models;

namespace VPT_user_api.Repos
{
    public class EventRepo
    {
        private readonly MySqlConnection db;

        public EventRepo(string connectionString)
        {
            db = new MySqlConnection(connectionString);
        }
        public List<Event> GetAllEvents()
        {
            List<Event> events = new List<Event>();

            try
            {
                db.Open();
               
                MySqlCommand eventQ = new MySqlCommand(@"
    SELECT 
        e.id, 
        e.location, 
        e.startdate, 
        e.enddate, 
        e.visitorlimit, 
        COUNT(ue.chair_id) AS chairs_reserved
    FROM 
        event e
    LEFT JOIN 
        user_event ue ON e.id = ue.event_id
    GROUP BY 
        e.id, 
        e.location, 
        e.startdate, 
        e.enddate, 
        e.visitorlimit", db);

                MySqlDataReader readEvents = eventQ.ExecuteReader();

                while (readEvents.Read())
                {
                    int eventId = (int)readEvents["id"];
                    var existingEvent = events.FirstOrDefault(e => e.Id == eventId);
                    if (existingEvent == null)
                    {
                        existingEvent = new Event
                        {
                            Id = eventId,
                            Location = (string)readEvents["location"],
                            StartDate = (DateTime)readEvents["startdate"],
                            EndDate = (DateTime)readEvents["enddate"],
                            VisitorLimit = Convert.ToInt32(readEvents["visitorlimit"]),
                            ChairsReserved = Convert.ToInt32(readEvents["chairs_reserved"]), // Assign the count to ChairsReserved
                        };
                        events.Add(existingEvent);
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
