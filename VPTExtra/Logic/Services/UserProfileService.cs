using Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Interfaces.Logic;
using Microsoft.Extensions.Logging;

namespace Logic.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ILogger<UserProfileService> _logger;
        private readonly IUserProfileDataRepository _profileDataRepo;
        public UserProfileService(IUserProfileDataRepository profileDataRepo, ILogger<UserProfileService> logger)
        {
            _logger = logger;
            _profileDataRepo = profileDataRepo;
        }
        public List<Event> RetrieveUserEvents(int userId)
        {
            return _profileDataRepo.RetrieveUserEvents(userId);
        }
    }
}
