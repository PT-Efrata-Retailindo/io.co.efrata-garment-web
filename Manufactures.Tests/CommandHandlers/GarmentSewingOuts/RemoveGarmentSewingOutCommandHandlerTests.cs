using Barebone.Tests;
using Manufactures.Application.GarmentSewingOuts.CommandHandlers;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using System.Linq;
using System.Linq.Expressions;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns;
using FluentAssertions;
using Manufactures.Domain.GarmentCuttingIns.Repositories;

namespace Manufactures.Tests.CommandHandlers.GarmentSewingOuts
{
    public class RemoveGarmentSewingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSewingOutRepository> _mockSewingOutRepository;
        private readonly Mock<IGarmentSewingOutItemRepository> _mockSewingOutItemRepository;
        private readonly Mock<IGarmentSewingOutDetailRepository> _mockSewingOutDetailRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;
        private readonly Mock<IGarmentCuttingInRepository> _mockCuttingInRepository;
        private readonly Mock<IGarmentCuttingInItemRepository> _mockCuttingInItemRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockCuttingInDetailRepository;

        public RemoveGarmentSewingOutCommandHandlerTests()
        {
            _mockSewingOutRepository = CreateMock<IGarmentSewingOutRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();
            _mockSewingOutDetailRepository = CreateMock<IGarmentSewingOutDetailRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();
            _mockCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();

            _MockStorage.SetupStorage(_mockSewingOutRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutDetailRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInRepository);
            _MockStorage.SetupStorage(_mockCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
        }
        private RemoveGarmentSewingOutCommandHandler CreateRemoveGarmentSewingOutCommandHandler()
        {
            return new RemoveGarmentSewingOutCommandHandler(_MockStorage.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingOutGuid = Guid.NewGuid();
            RemoveGarmentSewingOutCommandHandler unitUnderTest = CreateRemoveGarmentSewingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSewingOutCommand RemoveGarmentSewingOutCommand = new RemoveGarmentSewingOutCommand(sewingOutGuid);

            _mockSewingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingOutReadModel>()
                {
                    new GarmentSewingOutReadModel(sewingOutGuid)
                }.AsQueryable());
            _mockSewingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingOutItem>()
                {
                    new GarmentSewingOutItem(Guid.Empty, sewingOutGuid, Guid.Empty,sewingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            //_mockSewingOutDetailRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingOutDetailReadModel, bool>>>()))
            //    .Returns(new List<GarmentSewingOutDetail>()
            //    {
            //        new GarmentSewingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
            //    });

            _mockSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInItemReadModel>
                {
                    new GarmentSewingInItemReadModel(sewingInItemGuid)
                }.AsQueryable());

            _mockSewingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingOut>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingOutItem>()));
            //_mockSewingOutDetailRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentSewingOutDetail>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentSewingOutDetail>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSewingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
