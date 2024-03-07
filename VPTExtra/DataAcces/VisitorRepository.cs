using Interfaces;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces
{
    public class VisitorRepository : IVisitorRepository
    {
        private readonly MySqlConnection db;

        public VisitorRepository(string connectionString)
        {
            db = new MySqlConnection(connectionString);
        }
        public User GetVisitorById(int id)
        {
            User user = null;

            try
            {
                db.Open();

                MySqlCommand visitorQ = new MySqlCommand("SELECT * FROM user WHERE id = @VisitorId", db);
                visitorQ.Parameters.AddWithValue("@VisitorId", id);

                using (MySqlDataReader readUsers = visitorQ.ExecuteReader())
                {
                    if (readUsers.Read())
                    {
                        user = new User
                        {
                            Id = (int)readUsers["id"],
                            Name = readUsers["name"].ToString(),
                            Email = readUsers["email"].ToString(),
                        };
                    }
                }
            }
            finally
            {
                db.Close(); 
            }

            return user;
        }
    }
}
