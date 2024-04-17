﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Logic
{
    public interface IUserProfileService
    {
        public List<Event> RetrieveUserEvents(int userId);
    }
}
