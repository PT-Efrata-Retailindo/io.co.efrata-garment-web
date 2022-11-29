using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentPreparings.CommandHandlers;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentPreparings
{
    public class UpdateDatesGarmentPreparingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentPreparingRepository> _mockPreparingRepository;

        public UpdateDatesGarmentPreparingCommandHandlerTests()
        {
            _mockPreparingRepository = CreateMock<IGarmentPreparingRepository>();

            _MockStorage.SetupStorage(_mockPreparingRepository);
        }

        private UpdateDatesGarmentPreparingCommandHandler CreateUpdateDatesGarmentPreparingCommandHandler()
        {
            return new UpdateDatesGarmentPreparingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentPreparingCommandHandler unitUnderTest = CreateUpdateDatesGarmentPreparingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentPreparingCommand UpdateGarmentPreparingCommand = new UpdateDatesGarmentPreparingCommand(
                Ids, DateTimeOffset.Now);

            _mockPreparingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingReadModel>()
                {
                    new GarmentPreparingReadModel(guid)
                }.AsQueryable());

            _mockPreparingRepository
                .Setup(s => s.Update(It.IsAny<GarmentPreparing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentPreparing>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentPreparingCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
