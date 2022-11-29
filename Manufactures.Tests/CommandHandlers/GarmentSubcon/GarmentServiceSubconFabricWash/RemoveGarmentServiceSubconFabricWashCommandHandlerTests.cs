using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconFabricWashs
{
    public class RemoveGarmentServiceSubconFabricWashCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconFabricWashRepository> _mockServiceSubconFabricWashRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashItemRepository> _mockServiceSubconFabricWashItemRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashDetailRepository> _mockServiceSubconFabricWashDetailRepository;

        public RemoveGarmentServiceSubconFabricWashCommandHandlerTests()
        {
            _mockServiceSubconFabricWashRepository = CreateMock<IGarmentServiceSubconFabricWashRepository>();
            _mockServiceSubconFabricWashItemRepository = CreateMock<IGarmentServiceSubconFabricWashItemRepository>();
            _mockServiceSubconFabricWashDetailRepository = CreateMock<IGarmentServiceSubconFabricWashDetailRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconFabricWashRepository);
            _MockStorage.SetupStorage(_mockServiceSubconFabricWashItemRepository);
            _MockStorage.SetupStorage(_mockServiceSubconFabricWashDetailRepository);
        }

        private RemoveGarmentServiceSubconFabricWashCommandHandler CreateRemoveGarmentServiceSubconFabricWashCommandHandler()
        {
            return new RemoveGarmentServiceSubconFabricWashCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid serviceSubconFabricWashGuid = Guid.NewGuid();
            Guid serviceSubconFabricWashItemGuid = Guid.NewGuid();
            RemoveGarmentServiceSubconFabricWashCommandHandler unitUnderTest = CreateRemoveGarmentServiceSubconFabricWashCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentServiceSubconFabricWashCommand RemoveGarmentServiceSubconFabricWashCommand = new RemoveGarmentServiceSubconFabricWashCommand(serviceSubconFabricWashGuid);

            _mockServiceSubconFabricWashRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashReadModel>()
                {
                    new GarmentServiceSubconFabricWashReadModel(serviceSubconFabricWashGuid)
                }.AsQueryable());

            _mockServiceSubconFabricWashItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconFabricWashItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconFabricWashItem>()
                {
                    new GarmentServiceSubconFabricWashItem(
                        serviceSubconFabricWashItemGuid,
                        serviceSubconFabricWashGuid,
                        null,
                        DateTimeOffset.Now,
                        new UnitSenderId(1),
                        null,
                        null,
                        new UnitRequestId(1),
                        null,
                        null)
                });
            _mockServiceSubconFabricWashDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconFabricWashDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconFabricWashDetail>()
                {
                    new GarmentServiceSubconFabricWashDetail(
                        new Guid(),
                        serviceSubconFabricWashItemGuid,
                        new ProductId(1),
                        null,
                        null,
                        null,
                        null,
                        1,
                        new UomId(1),
                        null)
                });

            _mockServiceSubconFabricWashRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconFabricWash>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconFabricWash>()));
            _mockServiceSubconFabricWashItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconFabricWashItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconFabricWashItem>()));
            _mockServiceSubconFabricWashDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconFabricWashDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconFabricWashDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentServiceSubconFabricWashCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
