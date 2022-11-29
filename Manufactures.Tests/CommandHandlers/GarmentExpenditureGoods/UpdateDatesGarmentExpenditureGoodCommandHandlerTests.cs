using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentExpenditureGoods.CommandHandlers;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentExpenditureGoods
{
    public class UpdateDatesGarmentExpenditureGoodCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentExpenditureGoodRepository> _mockExpenditureGoodRepository;

        public UpdateDatesGarmentExpenditureGoodCommandHandlerTests()
        {
            _mockExpenditureGoodRepository = CreateMock<IGarmentExpenditureGoodRepository>();

            _MockStorage.SetupStorage(_mockExpenditureGoodRepository);
        }

        private UpdateDatesGarmentExpenditureGoodCommandHandler CreateUpdateDatesGarmentExpenditureGoodCommandHandler()
        {
            return new UpdateDatesGarmentExpenditureGoodCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentExpenditureGoodCommandHandler unitUnderTest = CreateUpdateDatesGarmentExpenditureGoodCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentExpenditureGoodCommand UpdateGarmentExpenditureGoodCommand = new UpdateDatesGarmentExpenditureGoodCommand(
                Ids, DateTimeOffset.Now);

            _mockExpenditureGoodRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodReadModel>()
                {
                    new GarmentExpenditureGoodReadModel(guid)
                }.AsQueryable());

            _mockExpenditureGoodRepository
                .Setup(s => s.Update(It.IsAny<GarmentExpenditureGood>()))
                .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGood>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentExpenditureGoodCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
