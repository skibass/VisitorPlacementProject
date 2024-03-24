using Interfaces.Repositories;
using Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Logic.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileDataRepository _profileDataRepo;
        public UserProfileService(IUserProfileDataRepository profileDataRepo)
        {
            _profileDataRepo = profileDataRepo;
        }
        public List<Event> RetrieveUserEvents(int userId)
        {
            return _profileDataRepo.RetrieveUserEvents(userId);
        }
    }
}
