using Interfaces.Logic;
using Interfaces.Repositories;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger) 
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        public User RegisterUser(User userToRegister)
        {
            try
            {
                var registeredUser = _userRepository.RegisterUser(userToRegister);
                _logger.LogInformation("Succesfully  registered user with email: {Email}", userToRegister.Email);
                return registeredUser;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error registering user with email: {Email} : {ErrorMessage}", userToRegister.Email, ex.Message);
                throw;
            }
        }
        public User LoginUser(User userToLogin)
        {
            try
            {
                var toLoginUser = _userRepository.LoginUser(userToLogin);

                _logger.LogInformation("Succesfully logged in user with email: {Email}", userToLogin.Email);

                return toLoginUser;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error logging in user with email: {Email} : {ErrorMessage}", userToLogin.Email, ex.Message);

                throw;
            }
        }
        public User GetVisitorById(int id)
        {
            try
            {
                var userToRetrieve = _userRepository.GetVisitorById(id);

                _logger.LogInformation("Succesfully retrieved user with id: {Id}", id);

                return userToRetrieve;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving user with Id: {Id} : {ErrorMessage}", id, ex.Message);

                throw;
            }
        }
    }
}
