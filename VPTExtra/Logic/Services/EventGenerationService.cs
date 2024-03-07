using Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class EventGenerationService
    {
        private readonly IEventManagement _eventManagement;
        private int VisitorsLeft;

        public EventGenerationService(IEventManagement eventManagement) 
        { 
            _eventManagement = eventManagement;
        }
        public Event GenerateEvent(Event _eventData, int amountParts, int amountRows)
        {
            Random rand = new();

            Event _event = new();

            _event.Location = "Locatie " + rand.Next(0, 10000);
            _event.StartDate = _eventData.StartDate;
            _event.EndDate = _eventData.EndDate;

            _event.VisitorLimit = _eventData.VisitorLimit;
            VisitorsLeft = _eventData.VisitorLimit;

            for (int i = 0; i < amountParts; i++)
            {
                GeneratePart(amountRows);
            }
            _eventManagement.CreateEvent(_event);
            return _event; 
        }
        private Part GeneratePart(int amountRowsPart)
        {
            Part part = new();
            part.Name = "test";

            for (int i = 0; i < amountRowsPart; i++)
            {
                GenerateRow();
            }
            return part;
        }
        private Row GenerateRow()
        {
            Row row = new();
            row.Name = "test";

            for (int i = 0; i < VisitorsLeft; i++)
            {
                GenerateChair();
                --VisitorsLeft;
            }
            return row;
        }
        private Chair GenerateChair()
        {
            Chair chair = new();
            chair.Name = "tets";

            return chair;
        }
    }
}
