using Interfaces;
using Interfaces.Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.EventGeneration
{
    public class EventGenerationService
    {
        private int ChairsLeft;
        private readonly IEventRepository _eventRepository;
        private readonly PartGenerationService _partGeneration;

        public EventGenerationService(IEventRepository eventManagement, PartGenerationService partGenerationService)
        {
            _eventRepository = eventManagement;
            _partGeneration = partGenerationService;
        }
        public Event GenerateEvent(Event currentEvent, int amountParts, int amountRows)
        {
            char partName = 'A';

            Random rand = new();

            Event newEvent = new();

            newEvent.Location = "Locatie " + rand.Next(0, 10000);
            newEvent.StartDate = currentEvent.StartDate;
            newEvent.EndDate = currentEvent.EndDate;
            newEvent.Parts = new List<Part>();

            newEvent.VisitorLimit = currentEvent.VisitorLimit;
            ChairsLeft = currentEvent.VisitorLimit;

            if (amountParts == 0)
            {
                while (ChairsLeft > 0)
                {
                    newEvent.Parts.Add(_partGeneration.GeneratePart(amountRows, partName.ToString(), ref ChairsLeft));
                    partName++;
                }
            }
            else
            {
                for (int i = 0; i < amountParts; i++)
                {
                    if (ChairsLeft != 0)
                    {
                        newEvent.Parts.Add(_partGeneration.GeneratePart(amountRows, partName.ToString(), ref ChairsLeft));
                        partName++;
                    }
                }
            }
            int actualAmountOfPlacedSeats = newEvent.VisitorLimit - ChairsLeft;

            newEvent.VisitorLimit = actualAmountOfPlacedSeats;

            _eventRepository.CreateEvent(newEvent);
            return newEvent;
        }      
    }
}
