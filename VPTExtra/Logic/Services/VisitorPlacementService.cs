using Interfaces.Logic;
using Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class VisitorPlacementService : IVisitorPlacementService
    {
        private readonly ILogger<VisitorPlacementService> _logger;
        private readonly IVisitorPlacementRepository _visitorPlacementRepository;
        public VisitorPlacementService(IVisitorPlacementRepository visitorPlacementRepository, ILogger<VisitorPlacementService> logger) 
        {
            _logger = logger;
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
