using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUserRepository
    {
        public User RegisterUser(User userToRegister);
        public User LoginUser(User userToRegister);
        public User GetVisitorById(int id);
    }
}
