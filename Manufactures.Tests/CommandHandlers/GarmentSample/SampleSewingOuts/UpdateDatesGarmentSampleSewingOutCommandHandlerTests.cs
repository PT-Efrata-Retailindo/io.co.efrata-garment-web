using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleSewingOuts.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleSewingOuts
{
    public class UpdateDatesGarmentSampleSewingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleSewingOutRepository> _mockSewingOutRepository;

        public UpdateDatesGarmentSampleSewingOutCommandHandlerTests()
        {
            _mockSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();

            _MockStorage.SetupStorage(_mockSewingOutRepository);
        }

        private UpdateDatesGarmentSampleSewingOutCommandHandler CreateUpdateDatesGarmentSampleSewingOutCommandHandler()
        {
            return new UpdateDatesGarmentSampleSewingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(sewingOutGuid.ToString());
            UpdateDatesGarmentSampleSewingOutCommandHandler unitUnderTest = CreateUpdateDatesGarmentSampleSewingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentSampleSewingOutCommand UpdateGarmentSampleSewingOutCommand = new UpdateDatesGarmentSampleSewingOutCommand(
                Ids, DateTimeOffset.Now);

            _mockSewingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingOutReadModel>()
                {
                    new GarmentSampleSewingOutReadModel(sewingOutGuid)
                }.AsQueryable());

            _mockSewingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOut>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSampleSewingOutCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
