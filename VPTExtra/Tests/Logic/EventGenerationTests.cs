﻿using Interfaces.Repositories;
using Logic.Services;
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
        [TestMethod]
        public void GenerateEvent_DefaultParameters_CreatesEventWithExpectedStructure()
        {
            // Arrange
            Event @event = new Event();
            @event.VisitorLimit = 30;

            var mockEventRepository = new Mock<IEventRepository>();
            var eventGenerationService = new EventGenerationService(mockEventRepository.Object);

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
            var eventGenerationService = new EventGenerationService(mockEventRepository.Object);

            // Act
            var generatedEvent = eventGenerationService.GenerateEvent(@event, amountParts: 2, amountRows: 3);

            // Assert
            Assert.IsNotNull(generatedEvent);
            Assert.AreEqual(0, generatedEvent.VisitorLimit);
            Assert.AreEqual(0, generatedEvent.Parts.Count);
        }
    }
}
