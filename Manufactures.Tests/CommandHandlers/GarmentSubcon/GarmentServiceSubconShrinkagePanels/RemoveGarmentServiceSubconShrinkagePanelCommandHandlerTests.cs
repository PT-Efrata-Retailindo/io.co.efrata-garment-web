using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconShrinkagePanels.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconShrinkagePanels
{
    public class RemoveGarmentServiceSubconShrinkagePanelCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconShrinkagePanelRepository> _mockServiceSubconShrinkagePanelRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelItemRepository> _mockServiceSubconShrinkagePanelItemRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelDetailRepository> _mockServiceSubconShrinkagePanelDetailRepository;

        public RemoveGarmentServiceSubconShrinkagePanelCommandHandlerTests()
        {
            _mockServiceSubconShrinkagePanelRepository = CreateMock<IGarmentServiceSubconShrinkagePanelRepository>();
            _mockServiceSubconShrinkagePanelItemRepository = CreateMock<IGarmentServiceSubconShrinkagePanelItemRepository>();
            _mockServiceSubconShrinkagePanelDetailRepository = CreateMock<IGarmentServiceSubconShrinkagePanelDetailRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconShrinkagePanelRepository);
            _MockStorage.SetupStorage(_mockServiceSubconShrinkagePanelItemRepository);
            _MockStorage.SetupStorage(_mockServiceSubconShrinkagePanelDetailRepository);
        }

        private RemoveGarmentServiceSubconShrinkagePanelCommandHandler CreateRemoveGarmentServiceSubconShrinkagePanelCommandHandler()
        {
            return new RemoveGarmentServiceSubconShrinkagePanelCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid serviceSubconShrinkagePanelGuid = Guid.NewGuid();
            Guid serviceSubconShrinkagePanelItemGuid = Guid.NewGuid();
            RemoveGarmentServiceSubconShrinkagePanelCommandHandler unitUnderTest = CreateRemoveGarmentServiceSubconShrinkagePanelCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentServiceSubconShrinkagePanelCommand RemoveGarmentServiceSubconShrinkagePanelCommand = new RemoveGarmentServiceSubconShrinkagePanelCommand(serviceSubconShrinkagePanelGuid);

            _mockServiceSubconShrinkagePanelRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelReadModel>()
                {
                    new GarmentServiceSubconShrinkagePanelReadModel(serviceSubconShrinkagePanelGuid)
                }.AsQueryable());

            _mockServiceSubconShrinkagePanelItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconShrinkagePanelItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconShrinkagePanelItem>()
                {
                    new GarmentServiceSubconShrinkagePanelItem(
                        serviceSubconShrinkagePanelItemGuid,
                        serviceSubconShrinkagePanelGuid,
                        null,
                        DateTimeOffset.Now,
                        new UnitSenderId(1),
                        null,
                        null,
                        new UnitRequestId(1),
                        null,
                        null)
                });
            _mockServiceSubconShrinkagePanelDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconShrinkagePanelDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconShrinkagePanelDetail>()
                {
                    new GarmentServiceSubconShrinkagePanelDetail(
                        new Guid(),
                        serviceSubconShrinkagePanelItemGuid,
                        new ProductId(1),
                        null,
                        null,
                        null,
                        null,
                        1,
                        new UomId(1),
                        null)
                });

            _mockServiceSubconShrinkagePanelRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconShrinkagePanel>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconShrinkagePanel>()));
            _mockServiceSubconShrinkagePanelItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconShrinkagePanelItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconShrinkagePanelItem>()));
            _mockServiceSubconShrinkagePanelDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconShrinkagePanelDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconShrinkagePanelDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentServiceSubconShrinkagePanelCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
