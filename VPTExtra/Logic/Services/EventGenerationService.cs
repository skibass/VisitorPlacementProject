using Interfaces;
using Interfaces.Logic;
using Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Models;
using System.Data.Common;

namespace Logic.Services
{
    public class EventGenerationService : IEventGenerationService
    {
        private int ChairsLeft;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<EventGenerationService> _logger;

        public EventGenerationService(IEventRepository eventManagement, ILogger<EventGenerationService> logger)
        {
            _logger = logger;
            _eventRepository = eventManagement;
        }
        public Event GenerateEvent(Event currentEvent, int amountParts, int amountRows)
        {
            char partName = 'A';          

            DateTime? startDate = currentEvent.StartDate;
            DateTime? endDate = currentEvent.EndDate;
            List<Part> parts = new List<Part>();

            int visitorLimit = currentEvent.VisitorLimit;
            ChairsLeft = currentEvent.VisitorLimit;

            if (amountParts == 0)
            {
                while (ChairsLeft > 0)
                {
                    parts.Add(GeneratePart(amountRows, partName.ToString()));
                    partName++;
                }
            }
            else
            {
                for (int i = 0; i < amountParts; i++)
                {
                    if (ChairsLeft != 0)
                    {
                        parts.Add(GeneratePart(amountRows, partName.ToString()));
                        partName++;
                    }
                }
            }
            int actualAmountOfPlacedSeats = visitorLimit - ChairsLeft;

            visitorLimit = actualAmountOfPlacedSeats;

            Event newEvent = new(startDate, endDate, parts, visitorLimit);

            try
            {
                _eventRepository.CreateEvent(newEvent);
                _logger.LogInformation("Created a new event with visitor limit: {VisitorLimit}", newEvent.VisitorLimit);
            }
            catch (DbException ex)
            {
                _logger.LogError("Error creating event with visitor limit: {VisitorLimit} : Amount of parts: {Parts} : Amount of rows: {Rows} : {ErrorMessage}", newEvent.VisitorLimit, amountParts, amountRows, ex.Message);
                throw;
            }

            return newEvent;
        }
        private Part GeneratePart(int amountRowsPerPart, string partName)
        {
            int rowNumber = 1;
            int defaultAmountOfRows = 6;

            List<Row> rows = new List<Row>();

            if (amountRowsPerPart == 0)
            {
                for (int i = 0; i < defaultAmountOfRows; i++)
                {
                    if (ChairsLeft > 0)
                    {
                        rows.Add(GenerateRow(partName + rowNumber));
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
                        rows.Add(GenerateRow(partName + rowNumber));
                        rowNumber++;
                    }
                }
            }
            Part part = new(partName, rows);

            return part;
        }
        private Row GenerateRow(string rowName)
        {
            int chairNumber = 1;
            int chairsThisRow = 0;

            List<Chair> chairs = new List<Chair>();

            for (int i = 0; i < ChairsLeft; i++)
            {
                if (chairsThisRow < 5)
                {
                    chairs.Add(GenerateChair(rowName + "-" + chairNumber));
                    chairNumber++;
                    chairsThisRow++;
                }
            }
            ChairsLeft = ChairsLeft - chairsThisRow;
            Row row = new(rowName, chairs);

            return row;
        }
        private Chair GenerateChair(string chairName)
        {
            Chair chair = new(chairName);

            return chair;
        }
    }
}
