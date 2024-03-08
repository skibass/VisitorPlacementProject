using Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class EventGenerationService
    {
        private int ChairsLeft;
        private readonly IEventManagement _eventManagement;

        public EventGenerationService(IEventManagement eventManagement)
        {
            _eventManagement = eventManagement;
        }
        public Event GenerateEvent(Event _eventData, int amountParts, int amountRows)
        {
            char partName = 'A';          

            Random rand = new();

            Event _event = new();

            _event.Location = "Locatie " + rand.Next(0, 10000);
            _event.StartDate = _eventData.StartDate;
            _event.EndDate = _eventData.EndDate;
            _event.Parts = new List<Part>();

            _event.VisitorLimit = _eventData.VisitorLimit;
            ChairsLeft = _eventData.VisitorLimit;

            if (amountParts == 0)
            {
                while (ChairsLeft > 0)
                {
                    _event.Parts.Add(GeneratePart(amountRows, partName.ToString()));
                    partName++;
                }
            }
            else
            {
                for (int i = 0; i < amountParts; i++)
                {
                    if (ChairsLeft != 0)
                    {
                        _event.Parts.Add(GeneratePart(amountRows, partName.ToString()));
                        partName++;
                    }
                }
            }

            _eventManagement.CreateEvent(_event);
            return _event;
        }
        private Part GeneratePart(int amountRowsPart, string partName)
        {
            int rowNumber = 1;

            Part part = new();
            part.Name = partName;
            part.Rows = new List<Row>();

            if (amountRowsPart == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (ChairsLeft > 0)
                    {
                        part.Rows.Add(GenerateRow(partName + rowNumber));
                        rowNumber++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < amountRowsPart; i++)
                {
                    if (ChairsLeft > 0)
                    {
                        part.Rows.Add(GenerateRow(partName + rowNumber));
                        rowNumber++;
                    }
                }
            }

            return part;
        }
        private Row GenerateRow(string rowName)
        {
            int chairNumber = 1;
            int chairsThisRow = 0;
            Row row = new();
            row.Name = rowName;
            row.Chairs = new List<Chair>();

            for (int i = 0; i < ChairsLeft; i++)
            {
                if (chairsThisRow < 10)
                {
                    row.Chairs.Add(GenerateChair(row.Name + "-" + chairNumber));
                    chairNumber++;
                    --ChairsLeft;
                    chairsThisRow++;
                }
            }
            return row;
        }
        private Chair GenerateChair(string chairName)
        {
            Chair chair = new();
            chair.Name = chairName;

            return chair;
        }
    }
}
