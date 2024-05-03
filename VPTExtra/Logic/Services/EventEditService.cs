using Interfaces.DataAcces.Repositories;
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
    public class EventEditService : IEventEditService
    {
        private readonly ILogger<EventEditService> _logger;
        private readonly IEventEditRepository _eventEditRepo;
        public EventEditService(IEventEditRepository eventEditRepo, ILogger<EventEditService> logger)
        {
            _eventEditRepo = eventEditRepo;
            _logger = logger;
        }

        public Part AddPart(int eventId)
        {
            return _eventEditRepo.AddPart(eventId);
        }
        public Row AddRow(int partId)
        {
            return _eventEditRepo.AddRow(partId);
        }
        public Chair AddChair(int rowId, int eventId)
        {
            return _eventEditRepo.AddChair(rowId, eventId);
        }
    }
}
