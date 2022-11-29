using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleFinishingOuts.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleFinishingOuts
{
    public class UpdateDatesGarmentSampleFinishingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleFinishingOutRepository> _mockFinishingOutRepository;

        public UpdateDatesGarmentSampleFinishingOutCommandHandlerTests()
        {
            _mockFinishingOutRepository = CreateMock<IGarmentSampleFinishingOutRepository>();

            _MockStorage.SetupStorage(_mockFinishingOutRepository);
        }

        private UpdateDatesGarmentSampleFinishingOutCommandHandler CreateUpdateDatesGarmentSampleFinishingOutCommandHandler()
        {
            return new UpdateDatesGarmentSampleFinishingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentSampleFinishingOutCommandHandler unitUnderTest = CreateUpdateDatesGarmentSampleFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentSampleFinishingOutCommand UpdateGarmentSampleFinishingOutCommand = new UpdateDatesGarmentSampleFinishingOutCommand(
                Ids, DateTimeOffset.Now);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingOutReadModel>()
                {
                    new GarmentSampleFinishingOutReadModel(guid)
                }.AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOut>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSampleFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
