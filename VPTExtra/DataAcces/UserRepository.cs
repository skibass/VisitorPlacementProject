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
    public class UserRepository : IUserRepository
    {
        private readonly MySqlConnection db;
        public UserRepository(string connectionString)
        {
            db = new MySqlConnection(connectionString);
        }
        public User RegisterUser(User user)
        {
            return user;
        }
    }
}
