using Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class VisitorPlacementService
    {
        private readonly IVisitorPlacementRepository _visitorPlacementRepository;
        public VisitorPlacementService(IVisitorPlacementRepository visitorPlacementRepository) 
        {
            _visitorPlacementRepository = visitorPlacementRepository;
        }
        public void PlaceVisitor(int chairId, int visitorId, int eventId)
        {
            _visitorPlacementRepository.PlaceVisitor(chairId, visitorId, eventId);
        }
        public void RevertVisitorPlacement(int chairId, int visitorId, int eventId)
        {
            _visitorPlacementRepository.RevertVisitorPlacement(chairId, visitorId, eventId);
        }
    }
}
