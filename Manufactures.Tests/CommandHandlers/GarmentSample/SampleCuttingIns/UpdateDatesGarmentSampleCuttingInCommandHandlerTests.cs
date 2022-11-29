using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleCuttingIns.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleCuttingIns
{
    public class UpdateDatesGarmentSampleCuttingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleCuttingInRepository> _mockCuttingInRepository;

        public UpdateDatesGarmentSampleCuttingInCommandHandlerTests()
        {
            _mockCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();

            _MockStorage.SetupStorage(_mockCuttingInRepository);
        }

        private UpdateDatesGarmentSampleCuttingInCommandHandler CreateUpdateDatesGarmentSampleCuttingInCommandHandler()
        {
            return new UpdateDatesGarmentSampleCuttingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentSampleCuttingInCommandHandler unitUnderTest = CreateUpdateDatesGarmentSampleCuttingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentSampleCuttingInCommand UpdateGarmentSampleCuttingInCommand = new UpdateDatesGarmentSampleCuttingInCommand(
                Ids, DateTimeOffset.Now);

            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInReadModel>()
                {
                    new GarmentSampleCuttingInReadModel(guid)
                }.AsQueryable());

            _mockCuttingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingIn>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSampleCuttingInCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
