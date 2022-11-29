using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleFinishingOuts.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ValueObjects;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleFinishingOuts
{
    public class UpdateGarmentSampleFinishingOutCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleFinishingOutRepository> _mockFinishingOutRepository;
        private readonly Mock<IGarmentSampleFinishingOutItemRepository> _mockFinishingOutItemRepository;
        private readonly Mock<IGarmentSampleFinishingOutDetailRepository> _mockFinishingOutDetailRepository;
        private readonly Mock<IGarmentSampleFinishingInItemRepository> _mockFinishingInItemRepository;

        public UpdateGarmentSampleFinishingOutCommandHandlerTest()
        {
            _mockFinishingOutRepository = CreateMock<IGarmentSampleFinishingOutRepository>();
            _mockFinishingOutItemRepository = CreateMock<IGarmentSampleFinishingOutItemRepository>();
            _mockFinishingOutDetailRepository = CreateMock<IGarmentSampleFinishingOutDetailRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentSampleFinishingInItemRepository>();

            _MockStorage.SetupStorage(_mockFinishingOutRepository);
            _MockStorage.SetupStorage(_mockFinishingOutItemRepository);
            _MockStorage.SetupStorage(_mockFinishingOutDetailRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
        }
        private UpdateGarmentSampleFinishingOutCommandHandler CreateUpdateGarmentSampleFinishingOutCommandHandler()
        {
            return new UpdateGarmentSampleFinishingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingOutGuid = Guid.NewGuid();
            Guid finishingInId = Guid.NewGuid();
            UpdateGarmentSampleFinishingOutCommandHandler unitUnderTest = CreateUpdateGarmentSampleFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSampleFinishingOutCommand updateGarmentSampleFinishingOutCommand = new UpdateGarmentSampleFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(2, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = true,
                FinishingTo = "FINISHING",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleFinishingOutItemValueObject>
                {
                    new GarmentSampleFinishingOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        FinishingInId= finishingInId,
                        FinishingInItemId=finishingInItemGuid,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        Details = new List<GarmentSampleFinishingOutDetailValueObject>
                        {
                            new GarmentSampleFinishingOutDetailValueObject
                            {
                                Size=new SizeValueObject(1, "Size"),
                                Uom = new Uom(1, "UomUnit"),
                                Quantity=1
                            }
                        }
                    }
                },

            };
            updateGarmentSampleFinishingOutCommand.SetIdentity(finishingOutGuid);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingOutReadModel>()
                {
                    new GarmentSampleFinishingOutReadModel(finishingOutGuid)
                }.AsQueryable());
            _mockFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutItem>()
                {
                    new GarmentSampleFinishingOutItem(Guid.Empty, finishingOutGuid, Guid.Empty,finishingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            _mockFinishingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutDetail>()
                {
                    new GarmentSampleFinishingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
                });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingInItemReadModel>
                {
                    new GarmentSampleFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOut>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOutItem>()));
            _mockFinishingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOutDetail>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(updateGarmentSampleFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
