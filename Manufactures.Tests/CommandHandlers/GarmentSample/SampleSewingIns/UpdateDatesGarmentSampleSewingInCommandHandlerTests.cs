using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleSewingIns.CommandHandler;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleSewingIns
{
    public class UpdateDatesGarmentSampleSewingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleSewingInRepository> _mockSewingInRepository;

        public UpdateDatesGarmentSampleSewingInCommandHandlerTests()
        {
            _mockSewingInRepository = CreateMock<IGarmentSampleSewingInRepository>();

            _MockStorage.SetupStorage(_mockSewingInRepository);
        }

        private UpdateDatesGarmentSampleSewingInCommandHandler CreateUpdateDatesGarmentSampleSewingInCommandHandler()
        {
            return new UpdateDatesGarmentSampleSewingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentSampleSewingInCommandHandler unitUnderTest = CreateUpdateDatesGarmentSampleSewingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentSampleSewingInCommand UpdateGarmentSampleSewingInCommand = new UpdateDatesGarmentSampleSewingInCommand(
                Ids, DateTimeOffset.Now);

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingInReadModel>()
                {
                    new GarmentSampleSewingInReadModel(guid)
                }.AsQueryable());

            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingIn>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSampleSewingInCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
