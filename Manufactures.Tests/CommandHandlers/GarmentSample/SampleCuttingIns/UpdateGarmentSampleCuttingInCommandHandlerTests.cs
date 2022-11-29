using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleCuttingIns.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ValueObjects;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleCuttingIns
{
    public class UpdateGarmentSampleCuttingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleCuttingInRepository> _mockSampleCuttingInRepository;
        private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockSampleCuttingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockSampleCuttingInDetailRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockSamplePreparingItemRepository;

        public UpdateGarmentSampleCuttingInCommandHandlerTests()
        {
            _mockSampleCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockSampleCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
            _mockSampleCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

            _MockStorage.SetupStorage(_mockSampleCuttingInRepository);
            _MockStorage.SetupStorage(_mockSampleCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockSampleCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockSamplePreparingItemRepository);
        }

        private UpdateGarmentSampleCuttingInCommandHandler CreateUpdateGarmentSampleCuttingInCommandHandler()
        {
            return new UpdateGarmentSampleCuttingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid cuttingInGuid = Guid.NewGuid();
            Guid preparingItemGuid = Guid.NewGuid();
            UpdateGarmentSampleCuttingInCommandHandler unitUnderTest = CreateUpdateGarmentSampleCuttingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSampleCuttingInCommand UpdateGarmentSampleCuttingInCommand = new UpdateGarmentSampleCuttingInCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                CuttingInDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleCuttingInItemValueObject>
                {
                    new GarmentSampleCuttingInItemValueObject
                    {
                        Details = new List<GarmentSampleCuttingInDetailValueObject>
                        {
                            new GarmentSampleCuttingInDetailValueObject
                            {
                                PreparingItemId = preparingItemGuid,
                                Product = new Product(1, "ProductCode", "ProductName"),
                                PreparingUom = new Uom(1, "UomUnit"),
                                CuttingInUom = new Uom(2, "PCS"),
                                IsSave = true,
                            }
                        }
                    }
                },

            };
            UpdateGarmentSampleCuttingInCommand.SetIdentity(cuttingInGuid);

            _mockSampleCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInReadModel>()
                {
                    new GarmentSampleCuttingInReadModel(cuttingInGuid)
                }.AsQueryable());
            _mockSampleCuttingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingInItem>()
                {
                    new GarmentSampleCuttingInItem(Guid.Empty, Guid.Empty, Guid.Empty, 0, null,Guid.Empty,null)
                });
            _mockSampleCuttingInDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingInDetail>()
                {
                    new GarmentSampleCuttingInDetail(Guid.Empty, Guid.Empty, preparingItemGuid,Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, null, 0, new UomId(1), null, 0, new UomId(1), null, 0, 0,1,1,null)
                });

            _mockSamplePreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSamplePreparingItemReadModel>
                {
                    new GarmentSamplePreparingItemReadModel(preparingItemGuid)
                }.AsQueryable());

            _mockSampleCuttingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingIn>()));
            _mockSampleCuttingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInItem>()));
            _mockSampleCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInDetail>()));
            _mockSamplePreparingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSamplePreparingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSamplePreparingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSampleCuttingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
