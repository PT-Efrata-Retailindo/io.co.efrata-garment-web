using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentLoadings.CommandHandlers;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentLoadings
{
    public class UpdateDatesGarmentLoadingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentLoadingRepository> _mockLoadingRepository;

        public UpdateDatesGarmentLoadingCommandHandlerTests()
        {
            _mockLoadingRepository = CreateMock<IGarmentLoadingRepository>();

            _MockStorage.SetupStorage(_mockLoadingRepository);
        }

        private UpdateDatesGarmentLoadingCommandHandler CreateUpdateDatesGarmentLoadingCommandHandler()
        {
            return new UpdateDatesGarmentLoadingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentLoadingCommandHandler unitUnderTest = CreateUpdateDatesGarmentLoadingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentLoadingCommand UpdateGarmentLoadingCommand = new UpdateDatesGarmentLoadingCommand(
                Ids, DateTimeOffset.Now);

            _mockLoadingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentLoadingReadModel>()
                {
                    new GarmentLoadingReadModel(guid)
                }.AsQueryable());

            _mockLoadingRepository
                .Setup(s => s.Update(It.IsAny<GarmentLoading>()))
                .Returns(Task.FromResult(It.IsAny<GarmentLoading>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentLoadingCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
