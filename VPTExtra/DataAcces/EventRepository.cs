using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Models;
using MySql.Data.MySqlClient;

namespace DataAcces
{
    public class EventRepository : IEventRepository
    {
        private readonly MySqlConnection db;

        public EventRepository(string connectionString)
        {
            db = new MySqlConnection(connectionString);
        }

        public List<Event> GetAllEvents()
        {
            List<Event> events = new List<Event>();

            events.Clear();

            db.Open();

            MySqlCommand eventQ = new MySqlCommand("SELECT e.id AS event_id, e.location, e.startdate, e.enddate, e.visitorlimit, " +
                                           "p.id AS part_id, p.name AS part_name, " +
                                           "r.id AS row_id, r.name AS row_name, " +
                                           "c.id AS chair_id, c.name AS chair_name, c.row_id AS chair_row_id " +
                                           "FROM event e " +
                                           "LEFT JOIN part p ON e.id = p.event_id " +
                                           "LEFT JOIN row r ON p.id = r.part_id " +
                                           "LEFT JOIN chair c ON r.id = c.row_id", db);

            MySqlDataReader readEvents = eventQ.ExecuteReader();

            while (readEvents.Read())
            {
                int eventId = (int)readEvents["event_id"];
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
                        Parts = new List<Part>()
                    };
                    events.Add(existingEvent);
                }

                if (readEvents["part_id"] != DBNull.Value)
                {
                    int partId = (int)readEvents["part_id"];
                    var existingPart = existingEvent.Parts.FirstOrDefault(p => p.Id == partId);
                    if (existingPart == null)
                    {
                        existingPart = new Part
                        {
                            Id = partId,
                            Name = (string)readEvents["part_name"],
                            Rows = new List<Row>()
                        };
                        existingEvent.Parts.Add(existingPart);
                    }
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

                    existingRow.Chairs.Add(new Chair
                    {
                        Id = (int)readEvents["chair_id"],
                        Name = (string)readEvents["chair_name"]
                    });
                }
                
            }
            db.Close();

            return events;
        }
        public Event GetEventById(int id)
        {
            Event _event = new Event();           

            db.Open();

            MySqlCommand eventQ = new MySqlCommand("SELECT * FROM event WHERE id = @id", db);
            eventQ.Parameters.AddWithValue("@id", id);

            MySqlDataReader readEvents = eventQ.ExecuteReader();

            if (readEvents.Read())
            {
                _event = new Event();
                _event.Id = (int)readEvents["id"];
                _event.Location = (string?)readEvents["location"];
                _event.StartDate = (DateTime?)readEvents["startdate"];
                _event.EndDate = (DateTime?)readEvents["enddate"];
                _event.VisitorLimit = Convert.ToInt32(readEvents["visitorlimit"]);
            }
            db.Close();

            return _event;
        }

        public void AddEvent(Event newEvent)
        {
            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    string sql = "INSERT INTO Events (Name, Date) VALUES (@Name, @Date)";
            //    using (var command = new SqlCommand(sql, connection))
            //    {
            //        command.Parameters.AddWithValue("@Name", newEvent.Id);
            //        command.Parameters.AddWithValue("@Date", newEvent.Location);
            //        command.ExecuteNonQuery();
            //    }
            //}
        }

        public void UpdateEvent(Event updatedEvent)
        {
            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    string sql = "UPDATE Events SET Name = @Name, Date = @Date WHERE EventId = @EventId";
            //    using (var command = new SqlCommand(sql, connection))
            //    {
            //        command.Parameters.AddWithValue("@EventId", updatedEvent.Id);
            //        command.Parameters.AddWithValue("@Name", updatedEvent.Location);
            //        command.Parameters.AddWithValue("@Date", updatedEvent.EndDate);
            //        command.ExecuteNonQuery();
            //    }
            //}
        }

        public void DeleteEvent(int eventId)
        {
            db.Open();

            MySqlCommand eventQ = new MySqlCommand("delete FROM event WHERE id = @id", db);

            eventQ.Parameters.AddWithValue("@id", eventId);

            eventQ.ExecuteNonQuery();
            db.Close();
        }
    }
}
