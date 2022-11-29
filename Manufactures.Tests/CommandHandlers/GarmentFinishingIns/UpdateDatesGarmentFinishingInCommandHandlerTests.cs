using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentFinishingIns.CommandHandlers;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Commands;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentFinishingIns
{
    public class UpdateDatesGarmentFinishingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentFinishingInRepository> _mockFinishingInRepository;

        public UpdateDatesGarmentFinishingInCommandHandlerTests()
        {
            _mockFinishingInRepository = CreateMock<IGarmentFinishingInRepository>();

            _MockStorage.SetupStorage(_mockFinishingInRepository);
        }

        private UpdateDatesGarmentFinishingInCommandHandler CreateUpdateDatesGarmentFinishingInCommandHandler()
        {
            return new UpdateDatesGarmentFinishingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentFinishingInCommandHandler unitUnderTest = CreateUpdateDatesGarmentFinishingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentFinishingInCommand UpdateGarmentFinishingInCommand = new UpdateDatesGarmentFinishingInCommand(
                Ids, DateTimeOffset.Now, "CUTTING");

            _mockFinishingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInReadModel>()
                {
                    new GarmentFinishingInReadModel(guid)
                }.AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingIn>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentFinishingInCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
