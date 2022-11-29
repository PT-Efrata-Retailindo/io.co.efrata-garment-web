using Barebone.Tests;
using Manufactures.Application.GarmentSample.SampleDeliveryReturns.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Commands;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleDeliveryReturns.CommandHandlers
{
    public class RemoveGarmentSampleDeliveryReturnCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleDeliveryReturnRepository> _mockGarmentSampleDeliveryReturnRepository;
        private readonly Mock<IGarmentSampleDeliveryReturnItemRepository> _mockGarmentSampleDeliveryReturnItemRepository;
        private readonly Mock<IGarmentSamplePreparingRepository> _mockGarmentSamplePreparingRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockGarmentSamplePreparingItemRepository;

        public RemoveGarmentSampleDeliveryReturnCommandHandlerTests()
        {
            _mockGarmentSampleDeliveryReturnRepository = CreateMock<IGarmentSampleDeliveryReturnRepository>();
            _mockGarmentSampleDeliveryReturnItemRepository = CreateMock<IGarmentSampleDeliveryReturnItemRepository>();
            _mockGarmentSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
            _mockGarmentSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleDeliveryReturnRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleDeliveryReturnItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingItemRepository);
        }

        private RemoveGarmentSampleDeliveryReturnCommandHandler CreateRemoveGarmentSampleDeliveryReturnCommandHandler()
        {
            return new RemoveGarmentSampleDeliveryReturnCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            RemoveGarmentSampleDeliveryReturnCommandHandler unitUnderTest = CreateRemoveGarmentSampleDeliveryReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentSampleDeliveryReturnRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnReadModel, bool>>>()))
            .Returns(new List<GarmentSampleDeliveryReturn>()
            {
                new GarmentSampleDeliveryReturn(id,"drNo","roNo","article",1,"unitDONo",1,"preparingId",DateTimeOffset.Now,"returnType",new UnitDepartmentId(1),"unitCode","unitName",new StorageId(1),"storageName","storageCode",true)
            });

            _mockGarmentSampleDeliveryReturnItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnItemReadModel, bool>>>()))
            .Returns(new List<GarmentSampleDeliveryReturnItem>()
            {
               new GarmentSampleDeliveryReturnItem(id,id,1,1,"preparingItemId",new ProductId(1),"productCode","FABRIC","designColor","roNo",1,new UomId(1),"uomUnit")
            });

            _mockGarmentSampleDeliveryReturnItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentSampleDeliveryReturnItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSampleDeliveryReturnItem>()));

            GarmentSamplePreparingItem garmentSamplePreparingItem = new GarmentSamplePreparingItem(id, 1, new Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1), "productCode", "productName", "designColor", 1, new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1), "uomUnit", "FABRIC", 1, 1, id, null);

            _mockGarmentSamplePreparingItemRepository
                      .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingItemReadModel, bool>>>()))
            .Returns(new List<GarmentSamplePreparingItem>()
            {
                    garmentSamplePreparingItem
            });

            _mockGarmentSamplePreparingItemRepository
           .Setup(s => s.Update(It.IsAny<GarmentSamplePreparingItem>()))
           .Returns(Task.FromResult(It.IsAny<GarmentSamplePreparingItem>()));

            _mockGarmentSampleDeliveryReturnRepository
            .Setup(s => s.Update(It.IsAny<GarmentSampleDeliveryReturn>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSampleDeliveryReturn>()));

            _MockStorage
            .Setup(x => x.Save())
            .Verifiable();

            // Act
            RemoveGarmentSampleDeliveryReturnCommand request = new RemoveGarmentSampleDeliveryReturnCommand();
            request.SetId(id);

            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }

        [Fact]
        public async Task Handle_Throws_ValidationException()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            RemoveGarmentSampleDeliveryReturnCommandHandler unitUnderTest = CreateRemoveGarmentSampleDeliveryReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentSampleDeliveryReturnRepository
                      .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnReadModel, bool>>>()))
            .Returns(new List<GarmentSampleDeliveryReturn>()
            {

            });

            // Act
            RemoveGarmentSampleDeliveryReturnCommand request = new RemoveGarmentSampleDeliveryReturnCommand();
            request.SetId(id);

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => unitUnderTest.Handle(request, cancellationToken));
        }
    }
}
