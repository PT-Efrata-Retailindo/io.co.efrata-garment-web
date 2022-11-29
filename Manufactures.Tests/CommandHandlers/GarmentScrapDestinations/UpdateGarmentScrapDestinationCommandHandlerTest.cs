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
    public class UpdateGarmentScrapDestinationCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentScrapDestinationRepository> _mockGarmentScrapDestinationRepository;
        public UpdateGarmentScrapDestinationCommandHandlerTest()
        {
            _mockGarmentScrapDestinationRepository = CreateMock<IGarmentScrapDestinationRepository>();
            _MockStorage.SetupStorage(_mockGarmentScrapDestinationRepository);
        }


        private UpdateGarmentScrapDestinationCommandHandler CreateUpdateGarmentScrapDestinationCommandHandler()
        {
            return new UpdateGarmentScrapDestinationCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid identity = Guid.NewGuid();
            UpdateGarmentScrapDestinationCommandHandler unitUnderTest = CreateUpdateGarmentScrapDestinationCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
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
            UpdateGarmentScrapDestinationCommand updateGarmentScrapDestinationCommand = new UpdateGarmentScrapDestinationCommand()
            {
                Code = "codes",
                Name = "names",
                Description = "desss"
            };
            updateGarmentScrapDestinationCommand.SetIdentity(identity);

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(updateGarmentScrapDestinationCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
