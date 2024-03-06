using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcces.Interfaces;
using Models;

namespace DataAcces
{
    public class EventRepository : IEventRepository
    {
        
        private readonly string _connectionString;

        public EventRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Event GetEventById(int eventId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT EventId, Name, Date FROM Events WHERE EventId = @EventId";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EventId", eventId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Event
                            {
                                Id = reader.GetInt32(0),
                                Location = reader.GetString(1),
                                StartDate = reader.GetDateTime(2),
                                EndDate = reader.GetDateTime(2),
                                VisitorLimit = reader.GetInt32(3)
                            };
                        }
                        return null; // Event not found
                    }
                }
            }
        }

        public void AddEvent(Event newEvent)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Events (Name, Date) VALUES (@Name, @Date)";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", newEvent.Name);
                    command.Parameters.AddWithValue("@Date", newEvent.Date);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateEvent(Event updatedEvent)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "UPDATE Events SET Name = @Name, Date = @Date WHERE EventId = @EventId";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EventId", updatedEvent.EventId);
                    command.Parameters.AddWithValue("@Name", updatedEvent.Name);
                    command.Parameters.AddWithValue("@Date", updatedEvent.Date);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteEvent(int eventId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM Events WHERE EventId = @EventId";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EventId", eventId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
}
