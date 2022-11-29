using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleExpenditureGoods.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleExpenditureGoods
{
    public class UpdateDatesGarmentSampleExpenditureGoodCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleExpenditureGoodRepository> _mockExpenditureGoodRepository;

        public UpdateDatesGarmentSampleExpenditureGoodCommandHandlerTests()
        {
            _mockExpenditureGoodRepository = CreateMock<IGarmentSampleExpenditureGoodRepository>();

            _MockStorage.SetupStorage(_mockExpenditureGoodRepository);
        }

        private UpdateDatesGarmentSampleExpenditureGoodCommandHandler CreateUpdateDatesGarmentSampleExpenditureGoodCommandHandler()
        {
            return new UpdateDatesGarmentSampleExpenditureGoodCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            List<string> Ids = new List<string>();
            Ids.Add(guid.ToString());
            UpdateDatesGarmentSampleExpenditureGoodCommandHandler unitUnderTest = CreateUpdateDatesGarmentSampleExpenditureGoodCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateDatesGarmentSampleExpenditureGoodCommand UpdateGarmentSampleExpenditureGoodCommand = new UpdateDatesGarmentSampleExpenditureGoodCommand(
                Ids, DateTimeOffset.Now);

            _mockExpenditureGoodRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleExpenditureGoodReadModel>()
                {
                    new GarmentSampleExpenditureGoodReadModel(guid)
                }.AsQueryable());

            _mockExpenditureGoodRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleExpenditureGood>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleExpenditureGood>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSampleExpenditureGoodCommand, cancellationToken);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
