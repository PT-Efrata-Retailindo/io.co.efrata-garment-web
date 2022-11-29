using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentScrapDestinations.CommandHandler;
using Manufactures.Domain.GarmentScrapDestinations;
using Manufactures.Domain.GarmentScrapDestinations.Commands;
using Manufactures.Domain.GarmentScrapDestinations.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentScrapDestinations
{
    public class PlaceGarmentScrapDestinationCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentScrapDestinationRepository> _mockGarmentScrapDestinationRepository;
        public PlaceGarmentScrapDestinationCommandHandlerTest()
        {
            _mockGarmentScrapDestinationRepository = CreateMock<IGarmentScrapDestinationRepository>();
            _MockStorage.SetupStorage(_mockGarmentScrapDestinationRepository);
        }


        private PlaceGarmentScrapDestinationCommandHandler CreatePlaceGarmentScrapDestinationCommandHandler()
        {
            return new PlaceGarmentScrapDestinationCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            PlaceGarmentScrapDestinationCommandHandler unitUnderTest = CreatePlaceGarmentScrapDestinationCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guid = Guid.NewGuid();

            PlaceGarmentScrapDestinationCommand placeGarmentScrapDestinationCommand = new PlaceGarmentScrapDestinationCommand()
            {
                Code = "code",
                Name = "name",
                Description = "desc"
            };

            _mockGarmentScrapDestinationRepository
                .Setup(s => s.Update(It.IsAny<GarmentScrapDestination>()))
                .Returns(Task.FromResult(It.IsAny<GarmentScrapDestination>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentScrapDestinationCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
