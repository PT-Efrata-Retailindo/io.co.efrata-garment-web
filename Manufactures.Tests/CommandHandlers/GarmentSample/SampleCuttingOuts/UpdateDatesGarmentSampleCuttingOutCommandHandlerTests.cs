using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleCuttingOuts.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleCuttingOuts
{
    public class UpdateDatesGarmentSampleCuttingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleCuttingOutRepository> _mockCuttingOutRepository;

        public UpdateDatesGarmentSampleCuttingOutCommandHandlerTests()
        {
            _mockCuttingOutRepository = CreateMock<IGarmentSampleCuttingOutRepository>();

            _MockStorage.SetupStorage(_mockCuttingOutRepository);
        }

        private UpdateDatesGarmentSampleCuttingOutCommandHandler CreateUpdateDatesGarmentSampleCuttingOutCommandHandler()
        {
            return new UpdateDatesGarmentSampleCuttingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentSampleCuttingOutCommandHandler unitUnderTest = CreateUpdateDatesGarmentSampleCuttingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentSampleCuttingOutCommand UpdateGarmentSampleCuttingOutCommand = new UpdateDatesGarmentSampleCuttingOutCommand(
                Ids, DateTimeOffset.Now);

            _mockCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingOutReadModel>()
                {
                    new GarmentSampleCuttingOutReadModel(guid)
                }.AsQueryable());

            _mockCuttingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingOut>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSampleCuttingOutCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
