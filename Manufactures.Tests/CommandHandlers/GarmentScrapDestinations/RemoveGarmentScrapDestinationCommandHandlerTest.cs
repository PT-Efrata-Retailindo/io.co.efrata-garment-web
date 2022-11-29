using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentScrapDestinations.CommandHandler;
using Manufactures.Domain.GarmentScrapDestinations;
using Manufactures.Domain.GarmentScrapDestinations.Commands;
using Manufactures.Domain.GarmentScrapDestinations.ReadModels;
using Manufactures.Domain.GarmentScrapDestinations.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentScrapDestinations
{
    public class RemoveGarmentScrapDestinationCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentScrapDestinationRepository> _mockGarmentScrapDestinationRepository;

        public RemoveGarmentScrapDestinationCommandHandlerTest()
        {
            _mockGarmentScrapDestinationRepository = CreateMock<IGarmentScrapDestinationRepository>();
            _MockStorage.SetupStorage(_mockGarmentScrapDestinationRepository);
        }


        private RemoveGarmentScrapDestinationCommandHandler CreateRemoveGarmentScrapDestinationCommandHandler()
        {
            return new RemoveGarmentScrapDestinationCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            RemoveGarmentScrapDestinationCommandHandler unitUnderTest = CreateRemoveGarmentScrapDestinationCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid identity = Guid.NewGuid();

            RemoveGarmentScrapDestinationCommand removeGarmentAvalComponentCommand = new RemoveGarmentScrapDestinationCommand(identity);
            _mockGarmentScrapDestinationRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentScrapDestinationReadModel>
                {
                    new GarmentScrapDestination(identity,"code","name","description").GetReadModel()
                }.AsQueryable());

            _mockGarmentScrapDestinationRepository
              .Setup(s => s.Update(It.IsAny<GarmentScrapDestination>()))
              .Returns(Task.FromResult(It.IsAny<GarmentScrapDestination>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(removeGarmentAvalComponentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
