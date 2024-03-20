﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IVisitorPlacement
    {
        public void PlaceVisitor(int chairId, int visitorId, int eventId);
        public void RevertVisitorPlacement(int chairId, int visitorId, int eventId);
    }
}
