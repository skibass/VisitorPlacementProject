using Autofac.Extras.Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Repositories;
using Models;
using DataAcces;
using Logic;
using Logic.Services;
using Moq;
using System.Net.Sockets;

namespace Tests.DataAcces
{
    #region Mock help
    // https://www.youtube.com/watch?v=DwbYxP-etMY
    #endregion

    [TestClass]
    public class EventRepositoryTests
    {
        [TestMethod]
        public void GetAllEvents_ReturnsListOfEvents()
        {
            // Arrange
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IEventRepository>()
                    .Setup(r => r.GetAllEvents())
                    .Returns(GetSampleEvents());

                var cls = mock.Create<EventService>();
                var expected = GetSampleEvents();

                // Act
                var actual = cls.GetAllEvents();

               
                // Assert
                Assert.IsTrue(actual != null);
                Assert.AreEqual(expected.Count, actual.Count);               
            }
            
        }

        [TestMethod]
        public void DeleteEvent()
        {
            // Arrange
            var currentEvent = GetSampleEvents()[0];

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IEventRepository>()
                    .Setup(r => r.DeleteEvent(currentEvent.Id));

                var eventService = mock.Create<EventService>();

                // Act
                eventService.DeleteEvent(currentEvent.Id);

                // Assert
                mock.Mock<IEventRepository>().Verify(r => r.DeleteEvent(currentEvent.Id), Times.Once);
            }              
        }

        [TestMethod]
        public void AddEvent()
        {
            // Arrange
            var sampleEvent = GetSampleEvents()[0];

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IEventRepository>()
                    .Setup(r => r.CreateEvent(sampleEvent));

                var eventService = mock.Create<EventService>();

                // Act
                eventService.CreateEvent(sampleEvent);
                                                       
                // Assert
                mock.Mock<IEventRepository>()
                    .Verify(r => r.CreateEvent(sampleEvent), Times.Exactly(1));
            }
        }

        private List<Event> GetSampleEvents()
        {
            List<Chair> chairs = new List<Chair>
            {
                new Chair
                {
                    Name = "Foo12"
                }
            };
            List<Row> rows = new List<Row>
            {
                new Row
                {
                    Name="Foo1",
                    Chairs = chairs
                }
            };
            List<Part> parts = new List<Part>
            {
                new Part
                {
                    Name = "Foo",
                    Rows = rows
                }
            };

          
            List<Event> events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Location = "testLocation",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    VisitorLimit = 10,
                    Parts = parts
                },
                 new Event
                {
                    Id = 2,
                    Location = "testLocation1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    VisitorLimit = 100,
                    Parts = parts
                },
                  new Event
                {
                    Id = 3,
                    Location = "testLocation2",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    VisitorLimit = 130,
                    Parts = parts
                }
            };

            
            return events;
        }
    }
}
