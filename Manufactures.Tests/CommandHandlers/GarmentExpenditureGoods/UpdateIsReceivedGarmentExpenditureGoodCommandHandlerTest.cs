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
    public class UpdateIsReceivedGarmentExpenditureGoodCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentExpenditureGoodRepository> _mockExpenditureGoodRepository;
        private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockExpenditureGoodItemRepository;

        public UpdateIsReceivedGarmentExpenditureGoodCommandHandlerTest()
        {
            _mockExpenditureGoodRepository = CreateMock<IGarmentExpenditureGoodRepository>();
            _mockExpenditureGoodItemRepository = CreateMock<IGarmentExpenditureGoodItemRepository>();

            _MockStorage.SetupStorage(_mockExpenditureGoodRepository);
            _MockStorage.SetupStorage(_mockExpenditureGoodItemRepository);
        }

        private UpdateIsReceivedGarmentExpenditureGoodCommandHandler CreateUpdateIsReceivedGarmentExpenditureGoodCommandHandler()
        {
            return new UpdateIsReceivedGarmentExpenditureGoodCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            UpdateIsReceivedGarmentExpenditureGoodCommandHandler unitUnderTest = CreateUpdateIsReceivedGarmentExpenditureGoodCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            _mockExpenditureGoodRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentExpenditureGoodReadModel>()
               {
                    new GarmentExpenditureGoodReadModel(id)
               }.AsQueryable());

            _mockExpenditureGoodRepository
               .Setup(s => s.Update(It.IsAny<GarmentExpenditureGood>()))
               .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGood>()));

            _MockStorage
               .Setup(x => x.Save())
               .Verifiable();

            // Act
            UpdateIsReceivedGarmentExpenditureGoodCommand request = new UpdateIsReceivedGarmentExpenditureGoodCommand(id, true);
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
        }
}
