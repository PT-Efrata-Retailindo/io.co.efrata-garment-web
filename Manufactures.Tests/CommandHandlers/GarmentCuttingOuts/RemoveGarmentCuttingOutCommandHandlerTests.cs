using Barebone.Tests;
using Manufactures.Application.GarmentCuttingOuts.CommandHandlers;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using System.Linq.Expressions;
using System.Linq;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns;
using FluentAssertions;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs;

namespace Manufactures.Tests.CommandHandlers.GarmentCuttingOuts
{
    public class RemoveGarmentCuttingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentCuttingOutRepository> _mockCuttingOutRepository;
        private readonly Mock<IGarmentCuttingOutItemRepository> _mockCuttingOutItemRepository;
        private readonly Mock<IGarmentCuttingOutDetailRepository> _mockCuttingOutDetailRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentSewingDORepository> _mockSewingDORepository;
        private readonly Mock<IGarmentSewingDOItemRepository> _mockSewingDOItemRepository;

        public RemoveGarmentCuttingOutCommandHandlerTests()
        {
            _mockCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
            _mockCuttingOutItemRepository = CreateMock<IGarmentCuttingOutItemRepository>();
            _mockCuttingOutDetailRepository = CreateMock<IGarmentCuttingOutDetailRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockSewingDORepository = CreateMock<IGarmentSewingDORepository>();
            _mockSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();

            _MockStorage.SetupStorage(_mockCuttingOutRepository);
            _MockStorage.SetupStorage(_mockCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockCuttingOutDetailRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockSewingDORepository);
            _MockStorage.SetupStorage(_mockSewingDOItemRepository);
        }

        private RemoveGarmentCuttingOutCommandHandler CreateRemoveGarmentCuttingOutCommandHandler()
        {
            return new RemoveGarmentCuttingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid cuttingInGuid = Guid.NewGuid();
            Guid cuttingOutGuid = Guid.NewGuid();
            Guid cuttingInDetailGuid = Guid.NewGuid();
            Guid sewingDOGuid = Guid.NewGuid();
            Guid cuttingOutDetailGuid = Guid.NewGuid();
            Guid cuttingOutItemGuid = Guid.NewGuid();
            RemoveGarmentCuttingOutCommandHandler unitUnderTest = CreateRemoveGarmentCuttingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentCuttingOutCommand RemoveGarmentCuttingOutCommand = new RemoveGarmentCuttingOutCommand(cuttingOutGuid);

            _mockCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>()
                {
                    new GarmentCuttingOutReadModel(cuttingOutGuid)
                }.AsQueryable());
            _mockCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingOutItem>()
                {
                    new GarmentCuttingOutItem(cuttingOutItemGuid, cuttingInGuid, cuttingInDetailGuid, cuttingOutGuid,new ProductId(1), null, null, null, 0)
                });

            _mockCuttingOutDetailRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutDetailReadModel, bool>>>()))
               .Returns(new List<GarmentCuttingOutDetail>()
               {
                    new GarmentCuttingOutDetail(cuttingOutDetailGuid, Guid.Empty, new SizeId(1), null, null, 0, 0, new UomId(1), null, 0,0)
               });

            _mockCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>
                {
                    new GarmentCuttingInDetailReadModel(cuttingInDetailGuid)
                }.AsQueryable());

            GarmentSewingDO garmentSewingDO = new GarmentSewingDO(
                sewingDOGuid, null,cuttingOutGuid,new UnitDepartmentId(1),null,null,new UnitDepartmentId(1),null,
                null,null,null,new GarmentComodityId(1),null,null,DateTimeOffset.Now);
            _mockSewingDORepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingDOReadModel>
                {
                    garmentSewingDO.GetReadModel()
                }.AsQueryable());

            //_mockSewingDORepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingDOReadModel>()
            //    {
            //        new GarmentSewingDOReadModel(sewingDOGuid)
            //    }.AsQueryable());
            _mockSewingDOItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingDOItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingDOItem>()
                {
                    new GarmentSewingDOItem(Guid.Empty, sewingDOGuid, cuttingOutDetailGuid, cuttingOutItemGuid,new ProductId(1), null, null,null, new SizeId(1), null,0,new UomId(1), null,null,0,0,0)
                });

            _mockCuttingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingOut>()));
            _mockCuttingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingOutItem>()));
            _mockCuttingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingOutDetail>()));
            _mockCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingInDetail>()));

            _mockSewingDORepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingDO>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingDO>()));
            _mockSewingDOItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingDOItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingDOItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentCuttingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
