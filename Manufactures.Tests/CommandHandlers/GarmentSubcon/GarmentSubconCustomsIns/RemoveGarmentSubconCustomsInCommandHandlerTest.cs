using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentSubconCustomsIns.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconCustomsIns
{
    public class RemoveGarmentSubconCustomsInCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconCustomsInRepository> _mockSubconCustomsInRepository;
        private readonly Mock<IGarmentSubconCustomsInItemRepository> _mockSubconCustomsInItemRepository;
        public RemoveGarmentSubconCustomsInCommandHandlerTest()
        {
            _mockSubconCustomsInRepository = CreateMock<IGarmentSubconCustomsInRepository>();
            _mockSubconCustomsInItemRepository = CreateMock<IGarmentSubconCustomsInItemRepository>();

            _MockStorage.SetupStorage(_mockSubconCustomsInRepository);
            _MockStorage.SetupStorage(_mockSubconCustomsInItemRepository);
        }

        private RemoveGarmentSubconCustomsInCommandHandler CreateRemoveGarmentSubconCustomsInCommandHandler()
        {
            return new RemoveGarmentSubconCustomsInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SubconCustomsInGuid = Guid.NewGuid();
            Guid SubconCustomsInItemGuid = Guid.NewGuid();
            RemoveGarmentSubconCustomsInCommandHandler unitUnderTest = CreateRemoveGarmentSubconCustomsInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconCustomsInCommand removeGarmentSubconCustomsInCommand = new RemoveGarmentSubconCustomsInCommand(SubconCustomsInGuid);

            GarmentSubconCustomsIn garmentSubconCustomsIn = new GarmentSubconCustomsIn(
                SubconCustomsInGuid, "no", DateTimeOffset.Now, "type", "type", Guid.NewGuid(), "no", new Domain.Shared.ValueObjects.SupplierId(1), "code", "name","remark",false,"category");

            _mockSubconCustomsInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconCustomsInReadModel>()
                {
                    garmentSubconCustomsIn.GetReadModel()
                }.AsQueryable());

            _mockSubconCustomsInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCustomsIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCustomsIn>()));

            _mockSubconCustomsInItemRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconCustomsInItemReadModel, bool>>>()))
               .Returns(new List<GarmentSubconCustomsInItem>()
               {
                    new GarmentSubconCustomsInItem(Guid.Empty, SubconCustomsInGuid, new Domain.Shared.ValueObjects.SupplierId(1), "code", "name", 1, "no", 1)
               });
            _mockSubconCustomsInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCustomsInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCustomsInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(removeGarmentSubconCustomsInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
