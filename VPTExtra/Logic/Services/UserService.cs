using Interfaces.Repositories;
using Interfaces.Service;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public User RegisterUser(User userToRegister)
        {
            return _userRepository.RegisterUser(userToRegister);
        }
        public User LoginUser(User userToRegister)
        {
            return _userRepository.LoginUser(userToRegister);
        }
        public User GetVisitorById(int id)
        {
            return _userRepository.GetVisitorById(id);
        }
    }
}
