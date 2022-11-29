using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentFinishingOuts.CommandHandlers;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentFinishingOuts
{
    public class UpdateDatesGarmentFinishingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentFinishingOutRepository> _mockFinishingOutRepository;

        public UpdateDatesGarmentFinishingOutCommandHandlerTests()
        {
            _mockFinishingOutRepository = CreateMock<IGarmentFinishingOutRepository>();

            _MockStorage.SetupStorage(_mockFinishingOutRepository);
        }

        private UpdateDatesGarmentFinishingOutCommandHandler CreateUpdateDatesGarmentFinishingOutCommandHandler()
        {
            return new UpdateDatesGarmentFinishingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentFinishingOutCommandHandler unitUnderTest = CreateUpdateDatesGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentFinishingOutCommand UpdateGarmentFinishingOutCommand = new UpdateDatesGarmentFinishingOutCommand(
                Ids, DateTimeOffset.Now);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>()
                {
                    new GarmentFinishingOutReadModel(guid)
                }.AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOut>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
