using Interfaces.Repositories;
using Logic.Services.EventGeneration;
using Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Logic
{
    [TestClass]
    public class EventGenerationTests
    {
        private ChairGenerationService? chairGenerationService;
        private RowGenerationService? rowGenerationService;
        private PartGenerationService? partGenerationService;
        private EventGenerationService? eventGenerationService;
        private Mock<IEventRepository>? mockEventRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            mockEventRepository = new Mock<IEventRepository>();

            chairGenerationService = new ChairGenerationService();
            rowGenerationService = new RowGenerationService(chairGenerationService);
            partGenerationService = new PartGenerationService(rowGenerationService);
            eventGenerationService = new EventGenerationService(mockEventRepository.Object, partGenerationService);
        }

        [TestMethod]
        public void GenerateEventAccordingToParameters()
        {
            // Arrange
            Event @event = new Event();
            @event.VisitorLimit = 30;           

            // Act
            var generatedEvent = eventGenerationService.GenerateEvent(@event, amountParts: 2, amountRows: 3);

            // Assert
            Assert.IsNotNull(generatedEvent);
            Assert.AreEqual(2, generatedEvent.Parts.Count);

            // Assert parts
            foreach (var part in generatedEvent.Parts)
            {
                Assert.AreEqual(3, part.Rows.Count);

                // Assert rows
                foreach (var row in part.Rows)
                {
                    Assert.AreEqual(5, row.Chairs.Count);
                }
            }
        }

        [TestMethod]
        public void GenerateEventWithLowVisitorLimit()
        {
            Event @event = new Event();
            @event.VisitorLimit = 0;

            // Act
            var generatedEvent = eventGenerationService.GenerateEvent(@event, amountParts: 2, amountRows: 3);

            // Assert
            Assert.IsNotNull(generatedEvent);
            Assert.AreEqual(0, generatedEvent.VisitorLimit);
            Assert.AreEqual(0, generatedEvent.Parts.Count);
        }
    }
}
