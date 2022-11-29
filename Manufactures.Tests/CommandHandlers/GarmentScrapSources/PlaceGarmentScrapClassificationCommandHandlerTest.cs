using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentScrapSources.CommandHandler;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentScrapSources
{
    public class PlaceGarmentScrapSourceCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentScrapSourceRepository> _mockGarmentScrapSourceRepository;
        public PlaceGarmentScrapSourceCommandHandlerTest()
        {
            _mockGarmentScrapSourceRepository = CreateMock<IGarmentScrapSourceRepository>();
            _MockStorage.SetupStorage(_mockGarmentScrapSourceRepository);
        }


        private PlaceGarmentScrapSourceCommandHandler CreatePlaceGarmentScrapSourceCommandHandler()
        {
            return new PlaceGarmentScrapSourceCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            PlaceGarmentScrapSourceCommandHandler unitUnderTest = CreatePlaceGarmentScrapSourceCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guid = Guid.NewGuid();

            PlaceGarmentScrapSourceCommand placeGarmentScrapSourceCommand = new PlaceGarmentScrapSourceCommand()
            {
                Code = "code",
                Name = "name",
                Description = "desc"
            };

            _mockGarmentScrapSourceRepository
                .Setup(s => s.Update(It.IsAny<GarmentScrapSource>()))
                .Returns(Task.FromResult(It.IsAny<GarmentScrapSource>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentScrapSourceCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
