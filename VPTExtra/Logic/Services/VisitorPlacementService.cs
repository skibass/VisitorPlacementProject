﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class VisitorPlacementService
    {
        private readonly IVisitorPlacement _visitorPlacementRepository;
        public VisitorPlacementService(IVisitorPlacement visitorPlacementRepository) 
        {
            _visitorPlacementRepository = visitorPlacementRepository;
        }
        public void PlaceVisitor(int chairId, int visitorId)
        {
            _visitorPlacementRepository.PlaceVisitor(chairId, visitorId);
        }

    }
}