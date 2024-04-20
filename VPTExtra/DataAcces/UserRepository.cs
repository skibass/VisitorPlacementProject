using Interfaces.Repositories;
using Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
            try
            {
                db.Open();

                // Check if user with the same email already exists
                User existingUser = GetVisitorByEmail(user.Email);
                if (existingUser != null)
                {
                    // User with the same email already exists
                    throw new Exception("User with this email already exists");
                }

                // Hash the user's password
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                // Insert the new user into the database
                MySqlCommand cmd = new MySqlCommand("INSERT INTO user (name, email, password) VALUES (@UserName, @Email, @Password)", db);
                cmd.Parameters.AddWithValue("@UserName", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                cmd.ExecuteNonQuery();

                return user;
            }
            finally
            {
                db.Close();
            }
        }
        public User LoginUser(User user)
        {
            try
            {
                db.Open();

                User retrievedUser = GetVisitorByEmail(user.Email);
                if (retrievedUser != null && BCrypt.Net.BCrypt.Verify(user.Password, retrievedUser.Password))
                {
                    return retrievedUser;
                }
                else
                {
                    throw new Exception("Email or password is wrong");
                }
            }
            catch (DbException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Close(); }
           
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
            catch (DbException ex)
            {
                throw new Exception(ex.Message);
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

            return user;
        }
    }
}
