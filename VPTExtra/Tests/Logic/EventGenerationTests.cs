using Interfaces.Repositories;
using Logic.Services;
using Microsoft.Extensions.Logging;
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
        private Mock<ILogger<EventGenerationService>> _loggerMock;
       
        [TestMethod]
        public void GenerateEvent_DefaultParameters_CreatesEventWithExpectedStructure()
        {
            // Arrange
            Event @event = new Event();
            @event.VisitorLimit = 30;
            _loggerMock = new Mock<ILogger<EventGenerationService>>();


            var mockEventRepository = new Mock<IEventRepository>();
            var eventGenerationService = new EventGenerationService(mockEventRepository.Object, _loggerMock.Object);

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
        public void GenerateEvent_LowVisitorLimit()
        {
            Event @event = new Event();
            @event.VisitorLimit = 0;

            var mockEventRepository = new Mock<IEventRepository>();
            _loggerMock = new Mock<ILogger<EventGenerationService>>();

            var eventGenerationService = new EventGenerationService(mockEventRepository.Object, _loggerMock.Object);

            // Act
            var generatedEvent = eventGenerationService.GenerateEvent(@event, amountParts: 2, amountRows: 3);

            // Assert
            Assert.IsNotNull(generatedEvent);
            Assert.AreEqual(0, generatedEvent.VisitorLimit);
            Assert.AreEqual(0, generatedEvent.Parts.Count);
        }
    }
}
