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

            MySqlCommand eventQ = new MySqlCommand("SELECT * from event", db);

            eventQ.ExecuteNonQuery();

            MySqlDataReader readEvents = eventQ.ExecuteReader();

            while (readEvents.Read())
            {
                Event _event = new Event();
                _event.Id = (int)readEvents["id"];
                _event.Location = (string?)readEvents["location"];
                _event.StartDate = (DateTime?)readEvents["startdate"];
                _event.EndDate = (DateTime?)readEvents["enddate"];
                _event.VisitorLimit = Convert.ToInt32(readEvents["visitorlimit"]);

                events.Add(_event);
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
            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    string sql = "DELETE FROM Events WHERE EventId = @EventId";
            //    using (var command = new SqlCommand(sql, connection))
            //    {
            //        command.Parameters.AddWithValue("@EventId", eventId);
            //        command.ExecuteNonQuery();
            //    }
            //}
        }
    }
}
