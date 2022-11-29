using Barebone.Tests;
using Manufactures.Application.GarmentSample.SampleCuttingOuts.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using System.Linq;
using System.Linq.Expressions;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleCuttingOuts
{
    public class RemoveGarmentSampleCuttingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleCuttingOutRepository> _mockCuttingOutRepository;
        private readonly Mock<IGarmentSampleCuttingOutItemRepository> _mockCuttingOutItemRepository;
        private readonly Mock<IGarmentSampleCuttingOutDetailRepository> _mockCuttingOutDetailRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentSampleSewingInRepository> _mockSewingDORepository;
        private readonly Mock<IGarmentSampleSewingInItemRepository> _mockSewingDOItemRepository;

        public RemoveGarmentSampleCuttingOutCommandHandlerTests()
        {
            _mockCuttingOutRepository = CreateMock<IGarmentSampleCuttingOutRepository>();
            _mockCuttingOutItemRepository = CreateMock<IGarmentSampleCuttingOutItemRepository>();
            _mockCuttingOutDetailRepository = CreateMock<IGarmentSampleCuttingOutDetailRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockSewingDORepository = CreateMock<IGarmentSampleSewingInRepository>();
            _mockSewingDOItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();

            _MockStorage.SetupStorage(_mockCuttingOutRepository);
            _MockStorage.SetupStorage(_mockCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockCuttingOutDetailRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockSewingDORepository);
            _MockStorage.SetupStorage(_mockSewingDOItemRepository);
        }

        private RemoveGarmentSampleCuttingOutCommandHandler CreateRemoveGarmentSampleCuttingOutCommandHandler()
        {
            return new RemoveGarmentSampleCuttingOutCommandHandler(_MockStorage.Object);
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
            RemoveGarmentSampleCuttingOutCommandHandler unitUnderTest = CreateRemoveGarmentSampleCuttingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSampleCuttingOutCommand RemoveGarmentSampleCuttingOutCommand = new RemoveGarmentSampleCuttingOutCommand(cuttingOutGuid);

            _mockCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingOutReadModel>()
                {
                    new GarmentSampleCuttingOutReadModel(cuttingOutGuid)
                }.AsQueryable());
            _mockCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingOutItem>()
                {
                    new GarmentSampleCuttingOutItem(cuttingOutItemGuid, cuttingInGuid, cuttingInDetailGuid, cuttingOutGuid,new ProductId(1), null, null, null, 0)
                });

            _mockCuttingOutDetailRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingOutDetailReadModel, bool>>>()))
               .Returns(new List<GarmentSampleCuttingOutDetail>()
               {
                    new GarmentSampleCuttingOutDetail(cuttingOutDetailGuid, Guid.Empty, new SizeId(1), null, null, 0, 0, new UomId(1), null, 0,0)
               });

            _mockCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInDetailReadModel>
                {
                    new GarmentSampleCuttingInDetailReadModel(cuttingInDetailGuid)
                }.AsQueryable());

            GarmentSampleSewingIn GarmentSampleSewingIn = new GarmentSampleSewingIn(
                sewingDOGuid,null, null, cuttingOutGuid,null, new UnitDepartmentId(1), null, null, new UnitDepartmentId(1), null,
                null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now);
            _mockSewingDORepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingInReadModel>
                {
                    GarmentSampleSewingIn.GetReadModel()
                }.AsQueryable());

            //_mockSewingDORepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSampleSewingInReadModel>()
            //    {
            //        new GarmentSampleSewingInReadModel(sewingDOGuid)
            //    }.AsQueryable());
            _mockSewingDOItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingInItem>()
                {
                    new GarmentSampleSewingInItem(Guid.Empty, sewingDOGuid, cuttingOutDetailGuid, cuttingOutItemGuid,Guid.Empty,Guid.Empty,new ProductId(1), null, null,null, new SizeId(1), null,0,new UomId(1), null,null,0,0,0)
                });

            _mockCuttingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingOut>()));
            _mockCuttingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingOutItem>()));
            _mockCuttingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingOutDetail>()));
            _mockCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInDetail>()));

            _mockSewingDORepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingIn>()));
            _mockSewingDOItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSampleCuttingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
