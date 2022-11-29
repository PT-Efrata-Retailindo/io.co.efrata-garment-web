using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalComponents.CommandHandlers;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.Commands;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.GarmentAvalComponents
{
    public class RemoveGarmentAvalComponentCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentAvalComponentRepository> _mockGarmentAvalComponentRepository;
        private readonly Mock<IGarmentAvalComponentItemRepository> _mockGarmentAvalComponentItemRepository;

        private readonly Mock<IGarmentCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
        private readonly Mock<IGarmentSewingOutItemRepository> _mockGarmentSewingOutItemRepository;

        public RemoveGarmentAvalComponentCommandHandlerTest()
        {
            _mockGarmentAvalComponentRepository = CreateMock<IGarmentAvalComponentRepository>();
            _mockGarmentAvalComponentItemRepository = CreateMock<IGarmentAvalComponentItemRepository>();

            _mockGarmentCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockGarmentSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentAvalComponentRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalComponentItemRepository);

            _MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);
        }

        private RemoveGarmentAvalComponentCommandHandler CreateRemoveGarmentAvalComponentCommandHandler()
        {
            return new RemoveGarmentAvalComponentCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_Cutting_ShouldSuccess()
        {
            // Arrange
            RemoveGarmentAvalComponentCommandHandler unitUnderTest = CreateRemoveGarmentAvalComponentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            RemoveGarmentAvalComponentCommand removeGarmentAvalComponentCommand = new RemoveGarmentAvalComponentCommand(avalComponentGuid);

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAvalComponentReadModel>()
                {
                    new GarmentAvalComponent(avalComponentGuid, "AvalComponentNo", new UnitDepartmentId(1), "UnitCode", "UnitName", "CUTTING", "RONo", "Article", new GarmentComodityId(1), "ComodityCode", "ComodityName", DateTimeOffset.Now, false).GetReadModel()
                }.AsQueryable());

            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalComponentItemReadModel, bool>>>()))
                .Returns(new List<GarmentAvalComponentItem>()
                {
                    new GarmentAvalComponentItem(Guid.Empty, avalComponentGuid, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, 0, new SizeId(1), null, 0,1)
                });

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Update(It.IsAny<GarmentAvalComponent>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAvalComponent>()));

            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentAvalComponentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAvalComponentItem>()));

            _mockGarmentCuttingInDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingInDetailReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingInDetail>()
                {
                    new GarmentCuttingInDetail(Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, new UomId(1), null, 0, new UomId(1), null, 0, 0, 1, 1, null)
                });

            _mockGarmentCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingInDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(removeGarmentAvalComponentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_CuttingInDetailNotFound_ShouldError()
        {
            // Arrange
            RemoveGarmentAvalComponentCommandHandler unitUnderTest = CreateRemoveGarmentAvalComponentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            RemoveGarmentAvalComponentCommand removeGarmentAvalComponentCommand = new RemoveGarmentAvalComponentCommand(avalComponentGuid);

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAvalComponentReadModel>()
                {
                    new GarmentAvalComponent(avalComponentGuid, "AvalComponentNo", new UnitDepartmentId(1), "UnitCode", "UnitName", "CUTTING", "RONo", "Article", new GarmentComodityId(1), "ComodityCode", "ComodityName", DateTimeOffset.Now, false).GetReadModel()
                }.AsQueryable());

            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalComponentItemReadModel, bool>>>()))
                .Returns(new List<GarmentAvalComponentItem>()
                {
                    new GarmentAvalComponentItem(Guid.Empty, avalComponentGuid, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, 0, new SizeId(1), null, 0,1)
                });

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Update(It.IsAny<GarmentAvalComponent>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAvalComponent>()));

            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentAvalComponentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAvalComponentItem>()));

            _mockGarmentCuttingInDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingInDetailReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingInDetail>());

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(removeGarmentAvalComponentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_Sewing_ShouldSuccess()
        {
            // Arrange
            RemoveGarmentAvalComponentCommandHandler unitUnderTest = CreateRemoveGarmentAvalComponentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            RemoveGarmentAvalComponentCommand removeGarmentAvalComponentCommand = new RemoveGarmentAvalComponentCommand(avalComponentGuid);

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAvalComponentReadModel>()
                {
                    new GarmentAvalComponent(avalComponentGuid, "AvalComponentNo", new UnitDepartmentId(1), "UnitCode", "UnitName", "SEWING", "RONo", "Article", new GarmentComodityId(1), "ComodityCode", "ComodityName", DateTimeOffset.Now, false).GetReadModel()
                }.AsQueryable());

            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalComponentItemReadModel, bool>>>()))
                .Returns(new List<GarmentAvalComponentItem>()
                {
                    new GarmentAvalComponentItem(Guid.Empty, avalComponentGuid, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, 0, new SizeId(1), null, 0,1)
                });

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Update(It.IsAny<GarmentAvalComponent>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAvalComponent>()));

            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentAvalComponentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAvalComponentItem>()));

            _mockGarmentSewingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingOutItem>()
                {
                    new GarmentSewingOutItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), "ProductCode", "ProductName", "DesignColor", new SizeId(1), "SizeName", 123, new UomId(1), "UomUnit", "Color", 123, 123, 123)
                });

            _mockGarmentSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(removeGarmentAvalComponentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_SewingOutItemNotFound_ShouldError()
        {
            // Arrange
            RemoveGarmentAvalComponentCommandHandler unitUnderTest = CreateRemoveGarmentAvalComponentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            RemoveGarmentAvalComponentCommand removeGarmentAvalComponentCommand = new RemoveGarmentAvalComponentCommand(avalComponentGuid);

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAvalComponentReadModel>()
                {
                    new GarmentAvalComponent(avalComponentGuid, "AvalComponentNo", new UnitDepartmentId(1), "UnitCode", "UnitName", "SEWING", "RONo", "Article", new GarmentComodityId(1), "ComodityCode", "ComodityName", DateTimeOffset.Now, false).GetReadModel()
                }.AsQueryable());

            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalComponentItemReadModel, bool>>>()))
                .Returns(new List<GarmentAvalComponentItem>()
                {
                    new GarmentAvalComponentItem(Guid.Empty, avalComponentGuid, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, 0, new SizeId(1), null, 0,1)
                });

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Update(It.IsAny<GarmentAvalComponent>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAvalComponent>()));

            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentAvalComponentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAvalComponentItem>()));

            _mockGarmentSewingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingOutItem>());

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(removeGarmentAvalComponentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
