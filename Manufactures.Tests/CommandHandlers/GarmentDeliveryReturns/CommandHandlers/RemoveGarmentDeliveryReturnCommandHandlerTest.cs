using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentDeliveryReturns.CommandHandlers;
using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.Commands;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentDeliveryReturns.CommandHandlers
{
    public class RemoveGarmentDeliveryReturnCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentDeliveryReturnRepository> _mockGarmentDeliveryReturnRepository;
        private readonly Mock<IGarmentDeliveryReturnItemRepository> _mockGarmentDeliveryReturnItemRepository;
        private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;

        public RemoveGarmentDeliveryReturnCommandHandlerTest()
        {
            _mockGarmentDeliveryReturnRepository = CreateMock<IGarmentDeliveryReturnRepository>();
            _mockGarmentDeliveryReturnItemRepository = CreateMock<IGarmentDeliveryReturnItemRepository>();
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentDeliveryReturnRepository);
            _MockStorage.SetupStorage(_mockGarmentDeliveryReturnItemRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);
        }

        private RemoveGarmentDeliveryReturnCommandHandler CreateRemoveGarmentDeliveryReturnCommandHandler()
        {
            return new RemoveGarmentDeliveryReturnCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            RemoveGarmentDeliveryReturnCommandHandler unitUnderTest = CreateRemoveGarmentDeliveryReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentDeliveryReturnRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnReadModel, bool>>>()))
            .Returns(new List<GarmentDeliveryReturn>()
            {
                new GarmentDeliveryReturn(id,"drNo","roNo","article",1,"unitDONo",1,"preparingId",DateTimeOffset.Now,"returnType",new UnitDepartmentId(1),"unitCode","unitName",new StorageId(1),"storageName","storageCode",true)
            });

            _mockGarmentDeliveryReturnItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnItemReadModel, bool>>>()))
            .Returns(new List<GarmentDeliveryReturnItem>()
            {
               new GarmentDeliveryReturnItem(id,id,1,1,"preparingItemId",new ProductId(1),"productCode","FABRIC","designColor","roNo",1,new UomId(1),"uomUnit","","","","","")
            });

            _mockGarmentDeliveryReturnItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentDeliveryReturnItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentDeliveryReturnItem>()));

            GarmentPreparingItem garmentPreparingItem = new GarmentPreparingItem(id, 1, new Domain.GarmentPreparings.ValueObjects.ProductId(1), "productCode", "productName", "designColor", 1, new Domain.GarmentPreparings.ValueObjects.UomId(1), "uomUnit", "FABRIC", 1, 1, id,null, "fasilitas");

            _mockGarmentPreparingItemRepository
                      .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingItemReadModel, bool>>>()))
            .Returns(new List<GarmentPreparingItem>()
            {
                    garmentPreparingItem
            });

            _mockGarmentPreparingItemRepository
           .Setup(s => s.Update(It.IsAny<GarmentPreparingItem>()))
           .Returns(Task.FromResult(It.IsAny<GarmentPreparingItem>()));

            _mockGarmentDeliveryReturnRepository
            .Setup(s => s.Update(It.IsAny<GarmentDeliveryReturn>()))
            .Returns(Task.FromResult(It.IsAny<GarmentDeliveryReturn>()));

            _MockStorage
            .Setup(x => x.Save())
            .Verifiable();

            // Act
            RemoveGarmentDeliveryReturnCommand request = new RemoveGarmentDeliveryReturnCommand();
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
            RemoveGarmentDeliveryReturnCommandHandler unitUnderTest = CreateRemoveGarmentDeliveryReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentDeliveryReturnRepository
                      .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnReadModel, bool>>>()))
            .Returns(new List<GarmentDeliveryReturn>()
            {

            });

            // Act
            RemoveGarmentDeliveryReturnCommand request = new RemoveGarmentDeliveryReturnCommand();
            request.SetId(id);

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => unitUnderTest.Handle(request, cancellationToken));
        }
    }
}
