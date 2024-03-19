using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class VisitorManagementService
    {
        private readonly IVisitorPlacement _visitorPlacementRepo;
        public VisitorManagementService(IVisitorPlacement visitorPlacement)
        {
            _visitorPlacementRepo = visitorPlacement;
        }

        public void PlaceVisitor(int chairId, int visitorId)
        {
            _visitorPlacementRepo.PlaceVisitor(chairId, visitorId);
        }
    }
}
