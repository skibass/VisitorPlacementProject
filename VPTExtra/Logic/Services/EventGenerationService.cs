using Interfaces;
using Interfaces.Repositories;
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
        private readonly IEventRepository _eventRepository;

        public EventGenerationService(IEventRepository eventManagement)
        {
            _eventRepository = eventManagement;
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
                    newEvent.Parts.Add(GeneratePart(amountRows, partName.ToString()));
                    partName++;
                }
            }
            else
            {
                for (int i = 0; i < amountParts; i++)
                {
                    if (ChairsLeft != 0)
                    {
                        newEvent.Parts.Add(GeneratePart(amountRows, partName.ToString()));
                        partName++;
                    }
                }
            }

            _eventRepository.CreateEvent(newEvent);
            return newEvent;
        }
        private Part GeneratePart(int amountRowsPerPart, string partName)
        {
            int rowNumber = 1;
            int defaultAmountOfRows = 6;

            Part part = new();
            part.Name = partName;
            part.Rows = new List<Row>();

            if (amountRowsPerPart == 0)
            {
                for (int i = 0; i < defaultAmountOfRows; i++)
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
                for (int i = 0; i < amountRowsPerPart; i++)
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
                if (chairsThisRow < 5)
                {
                    row.Chairs.Add(GenerateChair(row.Name + "-" + chairNumber));
                    chairNumber++;
                    chairsThisRow++;
                }
            }
            ChairsLeft = ChairsLeft - chairsThisRow;

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
