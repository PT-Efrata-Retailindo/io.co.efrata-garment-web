using Barebone.Tests;
using Manufactures.Application.GarmentFinishingIns.CommandHandlers;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Commands;
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
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentFinishingIns
{
    public class RemoveGarmentFinishingInCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentFinishingInRepository> _mockFinishingInRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockFinishingInItemRepository;
        private readonly Mock<IGarmentSewingOutItemRepository> _mockSewingOutItemRepository;

        public RemoveGarmentFinishingInCommandHandlerTest()
        {
            _mockFinishingInRepository = CreateMock<IGarmentFinishingInRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();

            _MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
        }

        private RemoveGarmentFinishingInCommandHandler CreateRemoveGarmentFinishingInCommandHandler()
        {
            return new RemoveGarmentFinishingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid loadingGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();
            Guid sewingOutGuid = Guid.NewGuid();
            RemoveGarmentFinishingInCommandHandler unitUnderTest = CreateRemoveGarmentFinishingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentFinishingInCommand RemoveGarmentFinishingInCommand = new RemoveGarmentFinishingInCommand(loadingGuid);

            _mockFinishingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInReadModel>()
                {
                    new GarmentFinishingInReadModel(loadingGuid)
                }.AsQueryable());
            _mockFinishingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingInItem>()
                {
                    new GarmentFinishingInItem(Guid.Empty, Guid.Empty,sewingOutItemGuid,Guid.Empty,Guid.Empty,new SizeId(1), null, new ProductId(1), null, null, null, 1,1,new UomId(1),null, null,1,1)
                });


            _mockSewingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingOutItemReadModel>
                {
                    new GarmentSewingOutItemReadModel(sewingOutItemGuid)
                }.AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingIn>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingInItem>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentFinishingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}