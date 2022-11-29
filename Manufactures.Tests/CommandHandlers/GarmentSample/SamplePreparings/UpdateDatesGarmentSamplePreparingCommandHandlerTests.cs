using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SamplePreparings.CommandHandlers;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SamplePreparings
{
    public class UpdateDatesGarmentSamplePreparingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSamplePreparingRepository> _mockPreparingRepository;

        public UpdateDatesGarmentSamplePreparingCommandHandlerTests()
        {
            _mockPreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();

            _MockStorage.SetupStorage(_mockPreparingRepository);
        }

        private UpdateDatesGarmentSamplePreparingCommandHandler CreateUpdateDatesGarmentSamplePreparingCommandHandler()
        {
            return new UpdateDatesGarmentSamplePreparingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentSamplePreparingCommandHandler unitUnderTest = CreateUpdateDatesGarmentSamplePreparingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentSamplePreparingCommand UpdateGarmentSamplePreparingCommand = new UpdateDatesGarmentSamplePreparingCommand(
                Ids, DateTimeOffset.Now);

            _mockPreparingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSamplePreparingReadModel>()
                {
                    new GarmentSamplePreparingReadModel(guid)
                }.AsQueryable());

            _mockPreparingRepository
                .Setup(s => s.Update(It.IsAny<GarmentSamplePreparing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSamplePreparing>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSamplePreparingCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
