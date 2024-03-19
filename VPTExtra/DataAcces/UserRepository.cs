using Interfaces;
using Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces
{
    public class UserRepository : IUserRepository
    {
        private readonly MySqlConnection db;
        public UserRepository(string connectionString)
        {
            db = new MySqlConnection(connectionString);
        }
        public User RegisterUser(User user)
        {
            db.Open();

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            MySqlCommand cmd = new MySqlCommand("INSERT INTO user (name, email, password) VALUES (@UserName, @Email, @Password)", db);

            cmd.Parameters.AddWithValue("@UserName", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);

            cmd.ExecuteNonQuery();

            db.Close();

            return user;
        }
        public User LoginUser(User user)
        {
            User retrievedUser = GetVisitorByEmail(user.Email);
            if (retrievedUser != null && BCrypt.Net.BCrypt.Verify(user.Password, retrievedUser.Password))
            {
                return retrievedUser;
            }
            return null;
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
                            Password = readUsers["password"].ToString(),
                            RoleId = (int)readUsers["roleid"],
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

        private User GetVisitorByEmail(string email)
        {
            User user = null;

            try
            {
                db.Open();

                MySqlCommand visitorQ = new MySqlCommand("SELECT * FROM user WHERE email = @Email", db);
                visitorQ.Parameters.AddWithValue("@Email", email);

                using (MySqlDataReader readUsers = visitorQ.ExecuteReader())
                {
                    if (readUsers.Read())
                    {
                        user = new User
                        {
                            Id = (int)readUsers["id"],
                            Name = readUsers["name"].ToString(),
                            Email = readUsers["email"].ToString(),
                            Password = readUsers["password"].ToString(),
                            RoleId = (int)readUsers["roleid"],
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
