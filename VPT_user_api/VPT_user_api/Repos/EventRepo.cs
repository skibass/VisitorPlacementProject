using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using VPTEventApi.Models;

namespace VPTEventApi.Repos
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

        public Event GetEventById(int id)
        {
            Event currentEvent = null;

            try
            {
                db.Open();

                MySqlCommand eventQ = new MySqlCommand("SELECT e.id AS event_id, e.location, e.startdate, e.enddate, e.visitorlimit, " +
                                            "p.id AS part_id, p.name AS part_name, " +
                                            "r.id AS row_id, r.name AS row_name, " +
                                            "c.id AS chair_id, c.name AS chair_name, c.row_id AS chair_row_id, c.user_id " +
                                            "FROM event e " +
                                            "LEFT JOIN part p ON e.id = p.event_id " +
                                            "LEFT JOIN row r ON p.id = r.part_id " +
                                            "LEFT JOIN chair c ON r.id = c.row_id " +
                                            "WHERE e.id = @eventId", db);
                eventQ.Parameters.AddWithValue("@eventId", id);

                MySqlDataReader readEvents = eventQ.ExecuteReader();

                while (readEvents.Read())
                {
                    if (currentEvent == null)
                    {
                        currentEvent = new Event
                        {
                            Id = id,
                            Location = (string)readEvents["location"],
                            StartDate = (DateTime)readEvents["startdate"],
                            EndDate = (DateTime)readEvents["enddate"],
                            VisitorLimit = Convert.ToInt32(readEvents["visitorlimit"]),
                            Parts = new List<Part>()
                        };
                    }

                    PopulateEvent(currentEvent, readEvents);
                }
            }
            finally
            {
                db.Close();
            }

            return currentEvent;
        }

        private void PopulateEvent(Event currentEvent, MySqlDataReader readEvents)
        {
            if (currentEvent == null)
            {
                currentEvent = new Event
                {
                    Id = (int)readEvents["event_id"],
                    Location = (string)readEvents["location"],
                    StartDate = (DateTime)readEvents["startdate"],
                    EndDate = (DateTime)readEvents["enddate"],
                    VisitorLimit = Convert.ToInt32(readEvents["visitorlimit"]),
                    Parts = new List<Part>()
                };
            }

            if (currentEvent.Parts == null)
            {
                currentEvent.Parts = new List<Part>();
            }
            if (readEvents["part_id"] != DBNull.Value)
            {
                int partId = (int)readEvents["part_id"];
                var existingPart = currentEvent.Parts.FirstOrDefault(p => p.Id == partId);
                if (existingPart == null)
                {
                    existingPart = new Part
                    {
                        Id = partId,
                        Name = (string)readEvents["part_name"],
                        Rows = new List<Row>()
                    };
                    currentEvent.Parts.Add(existingPart);
                }

                PopulateRow(existingPart, readEvents);
            }
        }

        private void PopulateRow(Part existingPart, MySqlDataReader readEvents)
        {
            if (readEvents["row_id"] != DBNull.Value)
            {
                int rowId = (int)readEvents["row_id"];

                var existingRow = existingPart.Rows.FirstOrDefault(r => r.Id == rowId);
                if (existingRow == null)
                {
                    existingRow = new Row
                    {
                        Id = rowId,
                        Name = (string)readEvents["row_name"],
                        Chairs = new List<Chair>()
                    };
                    existingPart.Rows.Add(existingRow);
                }

                PopulateChair(existingRow, readEvents);
            }
        }

        private void PopulateChair(Row existingRow, MySqlDataReader readEvents)
        {
            if (readEvents["chair_id"] != DBNull.Value)
            {
                if (readEvents["user_id"] != DBNull.Value)
                {
                    existingRow.Chairs.Add(new Chair
                    {
                        Id = (int)readEvents["chair_id"],
                        Name = (string)readEvents["chair_name"],
                        Uid = (int)readEvents["user_id"]
                    });
                }
                else
                {
                    existingRow.Chairs.Add(new Chair
                    {
                        Id = (int)readEvents["chair_id"],
                        Name = (string)readEvents["chair_name"]
                    });
                }
            }
        }
    }
}
