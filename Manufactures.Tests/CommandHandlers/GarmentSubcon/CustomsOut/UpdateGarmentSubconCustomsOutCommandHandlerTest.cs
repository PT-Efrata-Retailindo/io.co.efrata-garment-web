using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.CustomsOuts.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ValueObjects;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.CustomsOut
{
    public class UpdateGarmentSubconCustomsOutCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconCustomsOutRepository> _mockSubconCustomsOutRepository;
        private readonly Mock<IGarmentSubconCustomsOutItemRepository> _mockSubconCustomsOutItemRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockSubconDeliveryLetterOutRepository;
        public UpdateGarmentSubconCustomsOutCommandHandlerTest()
        {
            _mockSubconCustomsOutRepository = CreateMock<IGarmentSubconCustomsOutRepository>();
            _mockSubconCustomsOutItemRepository = CreateMock<IGarmentSubconCustomsOutItemRepository>();
            _mockSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();

            _MockStorage.SetupStorage(_mockSubconCustomsOutRepository);
            _MockStorage.SetupStorage(_mockSubconCustomsOutItemRepository);
            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutRepository);
        }

        private UpdateGarmentSubconCustomsOutCommandHandler CreateUpdateGarmentSubconCustomsOutCommandHandler()
        {
            return new UpdateGarmentSubconCustomsOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid SubconCustomsOutGuid = Guid.NewGuid();
            Guid SubconDLOutGuid = Guid.NewGuid();
            UpdateGarmentSubconCustomsOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconCustomsOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconCustomsOutCommand UpdateGarmentSubconCustomsOutCommand = new UpdateGarmentSubconCustomsOutCommand()
            {
                CustomsOutDate = DateTimeOffset.Now,
                CustomsOutNo = "no",
                Supplier = new Supplier
                {
                    Code = "test",
                    Id = 1,
                    Name = "test"
                },
                SubconType = "type",
                Items = new List<GarmentSubconCustomsOutItemValueObject>()
                {
                    new GarmentSubconCustomsOutItemValueObject
                    {
                       Quantity=1,
                       SubconDLOutId=SubconDLOutGuid,
                    }
                }

            };
            UpdateGarmentSubconCustomsOutCommand.SetIdentity(SubconCustomsOutGuid);

            _mockSubconCustomsOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconCustomsOutReadModel>()
                {
                    new GarmentSubconCustomsOutReadModel(SubconCustomsOutGuid)
                }.AsQueryable());


            _mockSubconCustomsOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconCustomsOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCustomsOutItem>()
                {
                    new GarmentSubconCustomsOutItem(Guid.Empty,SubconCustomsOutGuid,"",SubconDLOutGuid,1)
                });

            _mockSubconCustomsOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCustomsOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCustomsOutItem>()));

            _mockSubconCustomsOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCustomsOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCustomsOut>()));

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
                {
                    new GarmentSubconDeliveryLetterOut(SubconDLOutGuid,"","","SUBCON BAHAN BAKU",DateTimeOffset.Now,1,"","",1,"",false,"","",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel()
                }.AsQueryable());

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconCustomsOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
