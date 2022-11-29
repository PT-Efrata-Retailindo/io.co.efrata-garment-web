using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSewingIns.CommandHandlers;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingIns.ValueObjects;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
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


namespace Manufactures.Tests.CommandHandlers.GarmentSewingIns
{
    public class RemoveGarmentSewingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSewingInRepository> _mockSewingInRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;
        private readonly Mock<IGarmentLoadingItemRepository> _mockLoadingItemRepository;
        private readonly Mock<IGarmentSewingOutItemRepository> _mockSewingOutItemRepository;
        private readonly Mock<IGarmentFinishingOutItemRepository> _mockFinishingOutItemRepository;

        public RemoveGarmentSewingInCommandHandlerTests()
        {
            _mockSewingInRepository = CreateMock<IGarmentSewingInRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();
            _mockLoadingItemRepository = CreateMock<IGarmentLoadingItemRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();
            _mockFinishingOutItemRepository = CreateMock<IGarmentFinishingOutItemRepository>();

            _MockStorage.SetupStorage(_mockSewingInRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
            _MockStorage.SetupStorage(_mockLoadingItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockFinishingOutItemRepository);
        }

        private RemoveGarmentSewingInCommandHandler CreateRemoveGarmentSewingInCommandHandler()
        {
            return new RemoveGarmentSewingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_SEWING()
        {
            // Arrange
            Guid SewingInGuid = Guid.NewGuid();
            Guid preparingItemGuid = Guid.NewGuid();
            RemoveGarmentSewingInCommandHandler unitUnderTest = CreateRemoveGarmentSewingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSewingInCommand RemoveGarmentSewingInCommand = new RemoveGarmentSewingInCommand(SewingInGuid);
            Guid loadingItemGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();

            GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
                SewingInGuid,null,"SEWING",Guid.Empty, null,new UnitDepartmentId(1),null,null,
                new UnitDepartmentId(1),null,null,null,null, new GarmentComodityId(1),null,null,DateTimeOffset.Now);

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>()
                {
                    garmentSewingIn.GetReadModel()
                }.AsQueryable());
            _mockSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingInItem>()
                {
                    new GarmentSewingInItem(Guid.Empty, Guid.Empty,sewingOutItemGuid,Guid.Empty,loadingItemGuid,Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, new SizeId(1), null, 0, new UomId(1), null, null, 0,1,1)
                });

            //_mockLoadingItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentLoadingItemReadModel>
            //    {
            //        new GarmentLoadingItemReadModel(loadingItemGuid)
            //    }.AsQueryable());

            _mockSewingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingOutItemReadModel>
                {
                    new GarmentSewingOutItemReadModel(sewingOutItemGuid)
                }.AsQueryable());

            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingIn>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));
            //_mockLoadingItemRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentLoadingItem>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentLoadingItem>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSewingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_CUTTING()
        {
            // Arrange
            Guid SewingInGuid = Guid.NewGuid();
            Guid preparingItemGuid = Guid.NewGuid();
            RemoveGarmentSewingInCommandHandler unitUnderTest = CreateRemoveGarmentSewingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSewingInCommand RemoveGarmentSewingInCommand = new RemoveGarmentSewingInCommand(SewingInGuid);
            Guid loadingItemGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();

            GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
                SewingInGuid, null, "CUTTING", Guid.Empty, null, new UnitDepartmentId(1), null, null,
                new UnitDepartmentId(1), null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now);

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>()
                {
                    garmentSewingIn.GetReadModel()
                }.AsQueryable());
            _mockSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingInItem>()
                {
                    new GarmentSewingInItem(Guid.Empty, Guid.Empty,sewingOutItemGuid,Guid.Empty,loadingItemGuid,Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, new SizeId(1), null, 0, new UomId(1), null, null, 0,1,1)
                });

            //_mockLoadingItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentLoadingItemReadModel>
            //    {
            //        new GarmentLoadingItemReadModel(loadingItemGuid)
            //    }.AsQueryable());

            _mockLoadingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentLoadingItemReadModel>
                {
                    new GarmentLoadingItemReadModel(loadingItemGuid)
                }.AsQueryable());

            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingIn>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));
            //_mockLoadingItemRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentLoadingItem>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentLoadingItem>()));
            _mockLoadingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentLoadingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentLoadingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSewingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_FINISHING()
        {
            // Arrange
            Guid SewingInGuid = Guid.NewGuid();
            Guid preparingItemGuid = Guid.NewGuid();
            RemoveGarmentSewingInCommandHandler unitUnderTest = CreateRemoveGarmentSewingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSewingInCommand RemoveGarmentSewingInCommand = new RemoveGarmentSewingInCommand(SewingInGuid);
            Guid loadingItemGuid = Guid.NewGuid();
            Guid finishingOutOutItemGuid = Guid.NewGuid();

            GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
                SewingInGuid, null, "FINISHING", Guid.Empty, null, new UnitDepartmentId(1), null, null,
                new UnitDepartmentId(1), null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now);

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>()
                {
                    garmentSewingIn.GetReadModel()
                }.AsQueryable());
            _mockSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingInItem>()
                {
                    new GarmentSewingInItem(Guid.Empty, Guid.Empty,Guid.Empty,Guid.Empty,loadingItemGuid,finishingOutOutItemGuid,Guid.Empty, new ProductId(1), null, null, null, new SizeId(1), null, 0, new UomId(1), null, null, 0,1,1)
                });

            _mockFinishingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutItemReadModel>
                {
                    new GarmentFinishingOutItemReadModel(finishingOutOutItemGuid)
                }.AsQueryable());

            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingIn>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));
            //_mockLoadingItemRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentLoadingItem>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentLoadingItem>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSewingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}