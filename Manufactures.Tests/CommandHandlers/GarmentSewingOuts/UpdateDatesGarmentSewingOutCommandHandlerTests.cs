using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSewingOuts.CommandHandlers;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSewingOuts
{
    public class UpdateDatesGarmentSewingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSewingOutRepository> _mockSewingOutRepository;

        public UpdateDatesGarmentSewingOutCommandHandlerTests()
        {
            _mockSewingOutRepository = CreateMock<IGarmentSewingOutRepository>();

            _MockStorage.SetupStorage(_mockSewingOutRepository);
        }

        private UpdateDatesGarmentSewingOutCommandHandler CreateUpdateDatesGarmentSewingOutCommandHandler()
        {
            return new UpdateDatesGarmentSewingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(sewingOutGuid.ToString());
            UpdateDatesGarmentSewingOutCommandHandler unitUnderTest = CreateUpdateDatesGarmentSewingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentSewingOutCommand UpdateGarmentSewingOutCommand = new UpdateDatesGarmentSewingOutCommand(
                Ids, DateTimeOffset.Now );

            _mockSewingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingOutReadModel>()
                {
                    new GarmentSewingOutReadModel(sewingOutGuid)
                }.AsQueryable());

            _mockSewingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingOut>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSewingOutCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
