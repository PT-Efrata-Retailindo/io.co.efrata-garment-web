using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentCuttingOuts.CommandHandlers;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentCuttingOuts
{
    public class UpdateDatesGarmentCuttingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentCuttingOutRepository> _mockCuttingOutRepository;

        public UpdateDatesGarmentCuttingOutCommandHandlerTests()
        {
            _mockCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();

            _MockStorage.SetupStorage(_mockCuttingOutRepository);
        }

        private UpdateDatesGarmentCuttingOutCommandHandler CreateUpdateDatesGarmentCuttingOutCommandHandler()
        {
            return new UpdateDatesGarmentCuttingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentCuttingOutCommandHandler unitUnderTest = CreateUpdateDatesGarmentCuttingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentCuttingOutCommand UpdateGarmentCuttingOutCommand = new UpdateDatesGarmentCuttingOutCommand(
                Ids, DateTimeOffset.Now);

            _mockCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>()
                {
                    new GarmentCuttingOutReadModel(guid)
                }.AsQueryable());

            _mockCuttingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingOut>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentCuttingOutCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
