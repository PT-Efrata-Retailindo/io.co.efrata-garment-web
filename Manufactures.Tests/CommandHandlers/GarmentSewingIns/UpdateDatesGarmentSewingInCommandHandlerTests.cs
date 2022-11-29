using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSewingIns.CommandHandlers;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSewingIns
{
    public class UpdateDatesGarmentSewingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSewingInRepository> _mockSewingInRepository;

        public UpdateDatesGarmentSewingInCommandHandlerTests()
        {
            _mockSewingInRepository = CreateMock<IGarmentSewingInRepository>();

            _MockStorage.SetupStorage(_mockSewingInRepository);
        }

        private UpdateDatesGarmentSewingInCommandHandler CreateUpdateDatesGarmentSewingInCommandHandler()
        {
            return new UpdateDatesGarmentSewingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentSewingInCommandHandler unitUnderTest = CreateUpdateDatesGarmentSewingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentSewingInCommand UpdateGarmentSewingInCommand = new UpdateDatesGarmentSewingInCommand(
                Ids, DateTimeOffset.Now);

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>()
                {
                    new GarmentSewingInReadModel(guid)
                }.AsQueryable());

            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingIn>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSewingInCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
