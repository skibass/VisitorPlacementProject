using Autofac.Extras.Moq;
using Interfaces.Repositories;
using Logic.Services;
using Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DataAcces
{
    [TestClass]
    public class VisitorPlacementRepositoryTests
    {
        [TestMethod]
        public void PlaceVisitor()
        {
            // Arrange
            int chairId = 1;
            int visitorId = 1;
            int eventId = 1;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IVisitorPlacement>()
                .Setup(r => r.PlaceVisitor(chairId, visitorId, eventId));

                var eventService = mock.Create<VisitorPlacementService>();

                // Act
                eventService.PlaceVisitor(chairId, visitorId, eventId);

                // Assert
                mock.Mock<IVisitorPlacement>()
                    .Verify(r => r.PlaceVisitor(chairId, visitorId, eventId), Times.Exactly(1));
            }
        }
    }
}
