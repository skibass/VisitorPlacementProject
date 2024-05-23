using Interfaces;
using Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logic.Services.Tests
{
    [TestClass]
    public class EventGenerationServiceTests
    {
        private Mock<IEventRepository> _mockEventRepository;
        private Mock<ILogger<EventGenerationService>> _mockLogger;
        private EventGenerationService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _mockLogger = new Mock<ILogger<EventGenerationService>>();
            _service = new EventGenerationService(_mockEventRepository.Object, _mockLogger.Object);
        }

        [TestMethod]
        public void GenerateEvent_DefaultParameters_CreatesEventWithExpectedStructure()
        {
            // Arrange
            var currentEvent = new Event { VisitorLimit = 100 };

            // Act
            var generatedEvent = _service.GenerateEvent(currentEvent, amountParts: 2, amountRows: 3);

            // Assert
            Assert.IsNotNull(generatedEvent);
            Assert.AreEqual(2, generatedEvent.Parts.Count);

            foreach (var part in generatedEvent.Parts)
            {
                Assert.AreEqual(3, part.Rows.Count);
                foreach (var row in part.Rows)
                {
                    Assert.AreEqual(5, row.Chairs.Count);
                }
            }
        }

        [TestMethod]
        public void GenerateEvent_WithZeroPartsAndRows_CreatesDefaultPartsAndRows()
        {
            // Arrange
            var currentEvent = new Event { VisitorLimit = 100 };

            // Act
            var generatedEvent = _service.GenerateEvent(currentEvent, amountParts: 0, amountRows: 0);

            // Assert
            Assert.IsNotNull(generatedEvent);
            Assert.IsTrue(generatedEvent.Parts.Count > 0);

            foreach (var part in generatedEvent.Parts)
            {
                Assert.IsTrue(part.Rows.Count > 0);
                foreach (var row in part.Rows)
                {
                    Assert.IsTrue(row.Chairs.Count > 0);
                }
            }
        }

        [TestMethod]
        public void GenerateEvent_WithMoreRowsThanChairs_DoesNotExceedVisitorLimit()
        {
            // Arrange
            var currentEvent = new Event { VisitorLimit = 5 };

            // Act
            var generatedEvent = _service.GenerateEvent(currentEvent, amountParts: 1, amountRows: 10);

            // Assert
            Assert.IsNotNull(generatedEvent);
            Assert.AreEqual(1, generatedEvent.Parts.Count);
            Assert.AreEqual(1, generatedEvent.Parts[0].Rows.Count);
            Assert.AreEqual(5, generatedEvent.Parts[0].Rows[0].Chairs.Count);
        }

        [TestMethod]
        public void GenerateEvent_CreatesCorrectAmountOfPartsAndRows()
        {
            // Arrange
            var currentEvent = new Event { VisitorLimit = 100 };

            // Act
            var generatedEvent = _service.GenerateEvent(currentEvent, amountParts: 3, amountRows: 2);

            // Assert
            Assert.IsNotNull(generatedEvent);
            Assert.AreEqual(3, generatedEvent.Parts.Count);

            foreach (var part in generatedEvent.Parts)
            {
                Assert.AreEqual(2, part.Rows.Count);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerateEvent_LogsErrorOnDbException()
        {
            // Arrange
            var currentEvent = new Event { VisitorLimit = 100 };

            _mockEventRepository
                .Setup(repo => repo.CreateEvent(It.IsAny<Event>()))
                .Throws(new Exception("Database error"));

            // Act
            _service.GenerateEvent(currentEvent, amountParts: 1, amountRows: 5);

            // Assert
            _mockLogger.Verify(
                logger => logger.LogError(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()
                ),
                Times.Once
            );
        }
    }
}
