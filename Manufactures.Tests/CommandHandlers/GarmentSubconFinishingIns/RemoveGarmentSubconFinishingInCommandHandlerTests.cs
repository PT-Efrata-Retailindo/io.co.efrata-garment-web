using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentFinishingIns.CommandHandlers;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubconFinishingIns.Commands;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSubconFinishingIns
{
    public class RemoveGarmentSubconFinishingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentFinishingInRepository> _mockFinishingInRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockFinishingInItemRepository;
        private readonly Mock<IGarmentSubconCuttingRepository> _mockSubconCuttingRepository;

        public RemoveGarmentSubconFinishingInCommandHandlerTests()
        {
            _mockFinishingInRepository = CreateMock<IGarmentFinishingInRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();
            _mockSubconCuttingRepository = CreateMock<IGarmentSubconCuttingRepository>();

            _MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingRepository);
        }

        private RemoveGarmentSubconFinishingInCommandHandler CreateRemoveGarmentSubconFinishingInCommandHandler()
        {
            return new RemoveGarmentSubconFinishingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_Remove_Success()
        {
            // Arrange
            Guid finishingInGuid = Guid.NewGuid();
            Guid subconCuttingGuid = Guid.NewGuid();

            RemoveGarmentSubconFinishingInCommandHandler unitUnderTest = CreateRemoveGarmentSubconFinishingInCommandHandler();
            RemoveGarmentSubconFinishingInCommand RemoveGarmentFinishingInCommand = new RemoveGarmentSubconFinishingInCommand(finishingInGuid);
            CancellationToken cancellationToken = CancellationToken.None;

            _mockFinishingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInReadModel>()
                {
                    new GarmentFinishingInReadModel(finishingInGuid)
                }.AsQueryable());

            _mockFinishingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingInItem>()
                {
                    new GarmentFinishingInItem(Guid.Empty, Guid.Empty, Guid.Empty, Guid.NewGuid(), subconCuttingGuid, new SizeId(1), null, new ProductId(1), null, null, null, 1, 1, new UomId(1), null, null, 1, 1)
                });

            _mockSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconCuttingReadModel>
                {
                    new GarmentSubconCuttingReadModel(subconCuttingGuid)
                }.AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingIn>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingInItem>()));
            _mockSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCutting>()));

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
