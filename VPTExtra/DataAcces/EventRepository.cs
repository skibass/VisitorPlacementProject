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
        private readonly IVisitorRepository _visitorRepository;

        public EventRepository(string connectionString, IVisitorRepository visitorRepository)
        {
            db = new MySqlConnection(connectionString);
            _visitorRepository = visitorRepository;
        }

        private void PopulateEvent(Event _event, MySqlDataReader readEvents)
        {
            if (_event == null)
            {
                _event = new Event
                {
                    Id = (int)readEvents["event_id"],
                    Location = (string)readEvents["location"],
                    StartDate = (DateTime)readEvents["startdate"],
                    EndDate = (DateTime)readEvents["enddate"],
                    VisitorLimit = Convert.ToInt32(readEvents["visitorlimit"]),
                    Parts = new List<Part>()
                };
            }

            if (_event.Parts == null)
            {
                _event.Parts = new List<Part>();
            }
            if (readEvents["part_id"] != DBNull.Value)
            {

                int partId = (int)readEvents["part_id"];
                var existingPart = _event.Parts.FirstOrDefault(p => p.Id == partId);
                if (existingPart == null)
                {
                    existingPart = new Part
                    {
                        Id = partId,
                        Name = (string)readEvents["part_name"],
                        Rows = new List<Row>()
                    };
                    _event.Parts.Add(existingPart);
                }

                PopulateRow(existingPart, readEvents);
            }
        }

        private void PopulateRow(Part existingPart, MySqlDataReader readEvents)
        {
            int rowId = (int)readEvents["row_id"];
            if (readEvents["row_id"] != DBNull.Value)
            {
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
                        User = _visitorRepository.GetVisitorById((int)readEvents["user_id"])
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

        public List<Event> GetAllEvents()
        {
            List<Event> events = new List<Event>();

            db.Open();

            MySqlCommand eventQ = new MySqlCommand("SELECT e.id AS event_id, e.location, e.startdate, e.enddate, e.visitorlimit, " +
                                           "p.id AS part_id, p.name AS part_name, " +
                                           "r.id AS row_id, r.name AS row_name, " +
                                           "c.id AS chair_id, c.name AS chair_name, c.row_id AS chair_row_id, c.user_id " +
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

                PopulateEvent(existingEvent, readEvents);
            }
            db.Close();

            return events;
        }

        public Event GetEventById(int id)
        {
            Event _event = null;

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
                if (_event == null)
                {
                    _event = new Event
                    {
                        Id = id,
                        Location = (string)readEvents["location"],
                        StartDate = (DateTime)readEvents["startdate"],
                        EndDate = (DateTime)readEvents["enddate"],
                        VisitorLimit = Convert.ToInt32(readEvents["visitorlimit"]),
                        Parts = new List<Part>()
                    };
                }

                PopulateEvent(_event, readEvents);
            }

            db.Close();

            return _event;
        }

        public void CreateEvent(Event newEvent)
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
