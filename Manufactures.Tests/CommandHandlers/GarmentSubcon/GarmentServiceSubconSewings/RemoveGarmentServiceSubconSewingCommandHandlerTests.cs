using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconSewings.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconSewings
{
    public class RemoveGarmentServiceSubconSewingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconSewingRepository> _mockServiceSubconSewingRepository;
        private readonly Mock<IGarmentServiceSubconSewingItemRepository> _mockServiceSubconSewingItemRepository;
        private readonly Mock<IGarmentServiceSubconSewingDetailRepository> _mockServiceSubconSewingDetailRepository;

        public RemoveGarmentServiceSubconSewingCommandHandlerTests()
        {
            _mockServiceSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockServiceSubconSewingItemRepository = CreateMock<IGarmentServiceSubconSewingItemRepository>();
            _mockServiceSubconSewingDetailRepository = CreateMock<IGarmentServiceSubconSewingDetailRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconSewingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconSewingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSubconSewingDetailRepository);
        }

        private RemoveGarmentServiceSubconSewingCommandHandler CreateRemoveGarmentServiceSubconSewingCommandHandler()
        {
            return new RemoveGarmentServiceSubconSewingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid serviceSubconSewingGuid = Guid.NewGuid();
            Guid serviceSubconSewingItemGuid = Guid.NewGuid();
            RemoveGarmentServiceSubconSewingCommandHandler unitUnderTest = CreateRemoveGarmentServiceSubconSewingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentServiceSubconSewingCommand RemoveGarmentServiceSubconSewingCommand = new RemoveGarmentServiceSubconSewingCommand(serviceSubconSewingGuid);

            _mockServiceSubconSewingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingReadModel>()
                {
                    new GarmentServiceSubconSewingReadModel(serviceSubconSewingGuid)
                }.AsQueryable());

            _mockServiceSubconSewingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconSewingItem>()
                {
                    new GarmentServiceSubconSewingItem(
                        serviceSubconSewingItemGuid,
                        serviceSubconSewingGuid,
                        null,
                        null,
                        new GarmentComodityId(1),
                        null,
                        null,
                        new BuyerId(1),
                        null,
                        null,
                        new UnitDepartmentId(1),
                        null,
                        null)
                });
            _mockServiceSubconSewingDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconSewingDetail>()
                {
                    new GarmentServiceSubconSewingDetail(
                        new Guid(),
                        serviceSubconSewingItemGuid,
                        Guid.Empty,
                        Guid.Empty,
                        new ProductId(1),
                        null,
                        null,
                        null,
                        1,
                        new UomId(1),
                        null,
                        new UnitDepartmentId(1),
                        null,
                        null,
                        null,
                        null)
                });

            _mockServiceSubconSewingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewing>()));
            _mockServiceSubconSewingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewingItem>()));
            _mockServiceSubconSewingDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewingDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewingDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentServiceSubconSewingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
