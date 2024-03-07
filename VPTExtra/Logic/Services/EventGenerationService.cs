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
        private int VisitorsLeft;
        private readonly IEventManagement _eventManagement;

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
            _event.Parts = new List<Part>();

            _event.VisitorLimit = _eventData.VisitorLimit;
            VisitorsLeft = _eventData.VisitorLimit;

            for (int i = 0; i < amountParts; i++)
            {
                if (VisitorsLeft != 0)
                {
                    _event.Parts.Add(GeneratePart(amountRows));
                }
            }

            _eventManagement.CreateEvent(_event);
            return _event; 
        }
        private Part GeneratePart(int amountRowsPart)
        {
            Part part = new();
            part.Name = "test";
            part.Rows = new List<Row>();

            for (int i = 0; i < amountRowsPart; i++)
            {
                part.Rows.Add(GenerateRow());
            }
            return part;
        }
        private Row GenerateRow()
        {
            Row row = new();
            row.Name = "test";
            row.Chairs = new List<Chair>();

            for (int i = 0; i < VisitorsLeft; i++)
            {
                row.Chairs.Add(GenerateChair());
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
