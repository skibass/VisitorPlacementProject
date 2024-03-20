using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Repositories;
using Models;
using MySql.Data.MySqlClient;

namespace DataAcces
{
    public class EventRepository : IEventRepository
    {
        private readonly MySqlConnection db;
        private readonly IUserRepository _userRepository;

        public EventRepository(string connectionString, IUserRepository userRepository)
        {
            db = new MySqlConnection(connectionString);
            _userRepository = userRepository;
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
                        User = _userRepository.GetVisitorById((int)readEvents["user_id"])
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

            MySqlCommand eventQ = new MySqlCommand("SELECT id, location, startdate, enddate, visitorlimit from event", db);

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
                        Parts = new List<Part>()
                    };
                    events.Add(existingEvent);
                }
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
            db.Open();

            InsertEvent(newEvent);

            db.Close();
        }

        private void InsertEvent(Event newEvent)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO event (location, startdate, enddate, visitorlimit) VALUES (@Location, @StartDate, @EndDate, @VisitorLimit)", db);

            cmd.Parameters.AddWithValue("@Location", newEvent.Location);
            cmd.Parameters.AddWithValue("@StartDate", newEvent.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", newEvent.EndDate);
            cmd.Parameters.AddWithValue("@VisitorLimit", newEvent.VisitorLimit);

            cmd.ExecuteNonQuery();

            int eventId = (int)cmd.LastInsertedId;

            foreach (Part part in newEvent.Parts)
            {
                int partId = InsertPart(eventId, part);

                foreach (Row row in part.Rows)
                {
                    int rowId = InsertRow(partId, row);

                    foreach (Chair chair in row.Chairs)
                    {
                        InsertChair(rowId, chair);
                    }
                }
            }
        }

        private int InsertPart(int eventId, Part part)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO part (event_id, name) VALUES (@EventId, @Name)", db);

            cmd.Parameters.AddWithValue("@EventId", eventId);
            cmd.Parameters.AddWithValue("@Name", part.Name);

            cmd.ExecuteNonQuery();

            return (int)cmd.LastInsertedId;
        }

        private int InsertRow(int partId, Row row)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO row (part_id, name) VALUES (@PartId, @Name)", db);

            cmd.Parameters.AddWithValue("@PartId", partId);
            cmd.Parameters.AddWithValue("@Name", row.Name);

            cmd.ExecuteNonQuery();

            return (int)cmd.LastInsertedId;
        }

        private void InsertChair(int rowId, Chair chair)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO chair (row_id, name) VALUES (@RowId, @Name)", db);

            cmd.Parameters.AddWithValue("@RowId", rowId);
            cmd.Parameters.AddWithValue("@Name", chair.Name);

            cmd.ExecuteNonQuery();
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
