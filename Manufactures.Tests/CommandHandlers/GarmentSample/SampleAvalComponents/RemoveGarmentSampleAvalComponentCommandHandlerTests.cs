using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleAvalComponents.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleAvalComponents
{
    public class RemoveGarmentSampleAvalComponentCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleAvalComponentRepository> _mockGarmentSampleAvalComponentRepository;
        private readonly Mock<IGarmentSampleAvalComponentItemRepository> _mockGarmentSampleAvalComponentItemRepository;

        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockGarmentSampleCuttingInDetailRepository;
        private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockGarmentSampleSewingOutItemRepository;

        public RemoveGarmentSampleAvalComponentCommandHandlerTests()
        {
            _mockGarmentSampleAvalComponentRepository = CreateMock<IGarmentSampleAvalComponentRepository>();
            _mockGarmentSampleAvalComponentItemRepository = CreateMock<IGarmentSampleAvalComponentItemRepository>();

            _mockGarmentSampleCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockGarmentSampleSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleAvalComponentRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleAvalComponentItemRepository);

            _MockStorage.SetupStorage(_mockGarmentSampleCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleSewingOutItemRepository);
        }

        private RemoveGarmentSampleAvalComponentCommandHandler CreateRemoveGarmentSampleAvalComponentCommandHandler()
        {
            return new RemoveGarmentSampleAvalComponentCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_Cutting_ShouldSuccess()
        {
            // Arrange
            RemoveGarmentSampleAvalComponentCommandHandler unitUnderTest = CreateRemoveGarmentSampleAvalComponentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            RemoveGarmentSampleAvalComponentCommand removeGarmentSampleAvalComponentCommand = new RemoveGarmentSampleAvalComponentCommand(avalComponentGuid);

            _mockGarmentSampleAvalComponentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleAvalComponentReadModel>()
                {
                    new GarmentSampleAvalComponent(avalComponentGuid, "AvalComponentNo", new UnitDepartmentId(1), "UnitCode", "UnitName", "CUTTING", "RONo", "Article", new GarmentComodityId(1), "ComodityCode", "ComodityName", DateTimeOffset.Now, false).GetReadModel()
                }.AsQueryable());

            _mockGarmentSampleAvalComponentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalComponentItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleAvalComponentItem>()
                {
                    new GarmentSampleAvalComponentItem(Guid.Empty, avalComponentGuid, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, 0, new SizeId(1), null, 0,1)
                });

            _mockGarmentSampleAvalComponentRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleAvalComponent>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalComponent>()));

            _mockGarmentSampleAvalComponentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleAvalComponentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalComponentItem>()));

            _mockGarmentSampleCuttingInDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingInDetail>()
                {
                    new GarmentSampleCuttingInDetail(Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, new UomId(1), null, 0, new UomId(1), null, 0, 0, 1, 1, null)
                });

            _mockGarmentSampleCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(removeGarmentSampleAvalComponentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_CuttingInDetailNotFound_ShouldError()
        {
            // Arrange
            RemoveGarmentSampleAvalComponentCommandHandler unitUnderTest = CreateRemoveGarmentSampleAvalComponentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            RemoveGarmentSampleAvalComponentCommand removeGarmentSampleAvalComponentCommand = new RemoveGarmentSampleAvalComponentCommand(avalComponentGuid);

            _mockGarmentSampleAvalComponentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleAvalComponentReadModel>()
                {
                    new GarmentSampleAvalComponent(avalComponentGuid, "AvalComponentNo", new UnitDepartmentId(1), "UnitCode", "UnitName", "CUTTING", "RONo", "Article", new GarmentComodityId(1), "ComodityCode", "ComodityName", DateTimeOffset.Now, false).GetReadModel()
                }.AsQueryable());

            _mockGarmentSampleAvalComponentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalComponentItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleAvalComponentItem>()
                {
                    new GarmentSampleAvalComponentItem(Guid.Empty, avalComponentGuid, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, 0, new SizeId(1), null, 0,1)
                });

            _mockGarmentSampleAvalComponentRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleAvalComponent>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalComponent>()));

            _mockGarmentSampleAvalComponentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleAvalComponentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalComponentItem>()));

            _mockGarmentSampleCuttingInDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingInDetail>());

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(removeGarmentSampleAvalComponentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_Sewing_ShouldSuccess()
        {
            // Arrange
            RemoveGarmentSampleAvalComponentCommandHandler unitUnderTest = CreateRemoveGarmentSampleAvalComponentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            RemoveGarmentSampleAvalComponentCommand removeGarmentSampleAvalComponentCommand = new RemoveGarmentSampleAvalComponentCommand(avalComponentGuid);

            _mockGarmentSampleAvalComponentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleAvalComponentReadModel>()
                {
                    new GarmentSampleAvalComponent(avalComponentGuid, "AvalComponentNo", new UnitDepartmentId(1), "UnitCode", "UnitName", "SEWING", "RONo", "Article", new GarmentComodityId(1), "ComodityCode", "ComodityName", DateTimeOffset.Now, false).GetReadModel()
                }.AsQueryable());

            _mockGarmentSampleAvalComponentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalComponentItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleAvalComponentItem>()
                {
                    new GarmentSampleAvalComponentItem(Guid.Empty, avalComponentGuid, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, 0, new SizeId(1), null, 0,1)
                });

            _mockGarmentSampleAvalComponentRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleAvalComponent>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalComponent>()));

            _mockGarmentSampleAvalComponentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleAvalComponentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalComponentItem>()));

            _mockGarmentSampleSewingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingOutItem>()
                {
                    new GarmentSampleSewingOutItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), "ProductCode", "ProductName", "DesignColor", new SizeId(1), "SizeName", 123, new UomId(1), "UomUnit", "Color", 123, 123, 123)
                });

            _mockGarmentSampleSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(removeGarmentSampleAvalComponentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_SewingOutItemNotFound_ShouldError()
        {
            // Arrange
            RemoveGarmentSampleAvalComponentCommandHandler unitUnderTest = CreateRemoveGarmentSampleAvalComponentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            RemoveGarmentSampleAvalComponentCommand removeGarmentSampleAvalComponentCommand = new RemoveGarmentSampleAvalComponentCommand(avalComponentGuid);

            _mockGarmentSampleAvalComponentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleAvalComponentReadModel>()
                {
                    new GarmentSampleAvalComponent(avalComponentGuid, "AvalComponentNo", new UnitDepartmentId(1), "UnitCode", "UnitName", "SEWING", "RONo", "Article", new GarmentComodityId(1), "ComodityCode", "ComodityName", DateTimeOffset.Now, false).GetReadModel()
                }.AsQueryable());

            _mockGarmentSampleAvalComponentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalComponentItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleAvalComponentItem>()
                {
                    new GarmentSampleAvalComponentItem(Guid.Empty, avalComponentGuid, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, 0, new SizeId(1), null, 0,1)
                });

            _mockGarmentSampleAvalComponentRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleAvalComponent>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalComponent>()));

            _mockGarmentSampleAvalComponentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleAvalComponentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalComponentItem>()));

            _mockGarmentSampleSewingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingOutItem>());

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(removeGarmentSampleAvalComponentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
