using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleFinishingIns.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleFinishingIns
{
    public class UpdateDatesGarmentSampleFinishingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleFinishingInRepository> _mockFinishingInRepository;

        public UpdateDatesGarmentSampleFinishingInCommandHandlerTests()
        {
            _mockFinishingInRepository = CreateMock<IGarmentSampleFinishingInRepository>();

            _MockStorage.SetupStorage(_mockFinishingInRepository);
        }

        private UpdateDatesGarmentSampleFinishingInCommandHandler CreateUpdateDatesGarmentSampleFinishingInCommandHandler()
        {
            return new UpdateDatesGarmentSampleFinishingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentSampleFinishingInCommandHandler unitUnderTest = CreateUpdateDatesGarmentSampleFinishingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentSampleFinishingInCommand UpdateGarmentSampleFinishingInCommand = new UpdateDatesGarmentSampleFinishingInCommand(
                Ids, DateTimeOffset.Now, "CUTTING");

            _mockFinishingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingInReadModel>()
                {
                    new GarmentSampleFinishingInReadModel(guid)
                }.AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingIn>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSampleFinishingInCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
