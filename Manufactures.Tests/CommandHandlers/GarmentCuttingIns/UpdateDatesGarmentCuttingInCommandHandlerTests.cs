using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentCuttingIns.CommandHandlers;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Commands;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentCuttingIns
{
    public class UpdateDatesGarmentCuttingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentCuttingInRepository> _mockCuttingInRepository;

        public UpdateDatesGarmentCuttingInCommandHandlerTests()
        {
            _mockCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();

            _MockStorage.SetupStorage(_mockCuttingInRepository);
        }

        private UpdateDatesGarmentCuttingInCommandHandler CreateUpdateDatesGarmentCuttingInCommandHandler()
        {
            return new UpdateDatesGarmentCuttingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentCuttingInCommandHandler unitUnderTest = CreateUpdateDatesGarmentCuttingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentCuttingInCommand UpdateGarmentCuttingInCommand = new UpdateDatesGarmentCuttingInCommand(
                Ids, DateTimeOffset.Now);

            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInReadModel>()
                {
                    new GarmentCuttingInReadModel(guid)
                }.AsQueryable());

            _mockCuttingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingIn>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentCuttingInCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
