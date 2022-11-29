using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleSewingOuts.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ValueObjects;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleSewingOuts
{
    public class UpdateGarmentSampleSewingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleSewingOutRepository> _mockSewingOutRepository;
        private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockSewingOutItemRepository;
        private readonly Mock<IGarmentSampleSewingOutDetailRepository> _mockSewingOutDetailRepository;
        private readonly Mock<IGarmentSampleSewingInItemRepository> _mockSewingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInRepository> _mockCuttingInRepository;
        private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockCuttingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;

        public UpdateGarmentSampleSewingOutCommandHandlerTests()
        {
            _mockSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();
            _mockSewingOutDetailRepository = CreateMock<IGarmentSampleSewingOutDetailRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();
            _mockCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();

            _MockStorage.SetupStorage(_mockSewingOutRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutDetailRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInRepository);
            _MockStorage.SetupStorage(_mockCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockComodityPriceRepository);
        }
        private UpdateGarmentSampleSewingOutCommandHandler CreateUpdateGarmentSampleSewingOutCommandHandler()
        {
            return new UpdateGarmentSampleSewingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            Guid sewingOutGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentSampleSewingOutCommandHandler unitUnderTest = CreateUpdateGarmentSampleSewingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            GarmentSampleCuttingIn garmentCuttingIn = new GarmentSampleCuttingIn(
                  sewingOutGuid, "", "", "", "", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, 9);
            GarmentSampleCuttingInItem garmentCuttingInItem = new GarmentSampleCuttingInItem(
                  sewingOutGuid, sewingOutGuid, sewingOutGuid, 1, "", sewingOutGuid, "");
            GarmentSampleCuttingInDetail garmentCuttingInDetail = new GarmentSampleCuttingInDetail(
                  sewingOutGuid, sewingOutGuid, sewingOutGuid, sewingOutGuid, sewingOutGuid
                  , new ProductId(1), "code", "name", "color", "fab", 9, new UomId(1), "uom", 1, new UomId(1), "uom", 1, 90, 90, 2, "");
            GarmentSampleSewingInItem garmentSewingInItem = new GarmentSampleSewingInItem(
              sewingOutGuid, sewingOutGuid, sewingOutGuid, sewingOutGuid, Guid.Empty, Guid.Empty, new ProductId(1), "code", "name", "color", new SizeId(1), "code", 1, new UomId(1), "uom", "color", 0, 19, 19);
            GarmentSampleSewingOut garmentSewingOut = new GarmentSampleSewingOut(
                sewingOutGuid, "No", new BuyerId(1), "code", "name", new UnitDepartmentId(1), "code", "name", "CUTTING", DateTimeOffset.Now, "ro", "article", new UnitDepartmentId(1), " code", "name", new GarmentComodityId(1), "code", "name", true);


            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInReadModel>()
                {
                    garmentCuttingIn.GetReadModel()
                }.AsQueryable());

            _mockCuttingInItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleCuttingInItemReadModel>()
               {
                   garmentCuttingInItem.GetReadModel()
               }
               .AsQueryable());
            _mockCuttingInDetailRepository
              .Setup(s => s.Query)
              .Returns(new List<GarmentSampleCuttingInDetailReadModel>()
              {
                   garmentCuttingInDetail.GetReadModel()
              }
              .AsQueryable());

            _mockSewingInItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleSewingInItemReadModel>()
               {
                   garmentSewingInItem.GetReadModel()
               }
               .AsQueryable());

            _mockSewingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingOutReadModel>()
                {
                    garmentSewingOut.GetReadModel()
                }.AsQueryable());


            _mockCuttingInItemRepository
           .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInItemReadModel, bool>>>()))
           .Returns(new List<GarmentSampleCuttingInItem>()
           {
               new GarmentSampleCuttingInItem(sewingOutGuid,sewingOutGuid,sewingOutGuid,1,"",sewingOutGuid,"")
           });

            _mockSewingOutItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutItemReadModel, bool>>>()))
            .Returns(new List<GarmentSampleSewingOutItem>()
            {
               new GarmentSampleSewingOutItem(sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,new ProductId(1),"","","",new SizeId(1),"",1,new UomId(1),"","",1,10,10)
            });
            _mockSewingOutDetailRepository
          .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutDetailReadModel, bool>>>()))
          .Returns(new List<GarmentSampleSewingOutDetail>()
          {
                new GarmentSampleSewingOutDetail(sewingOutGuid,sewingOutGuid,new SizeId(1),"",1,new UomId(1),"")
          });
            _mockSewingOutRepository
               .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOut>()))
               .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOut>()));

            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingInItem>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutItem>()));
            _mockSewingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutDetail>()));
            _mockCuttingInRepository
               .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingIn>()))
               .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingIn>()));
            _mockCuttingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInItem>()));
            _mockCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInDetail>()));
            Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands.UpdateGarmentSampleSewingOutCommand UpdateGarmentSampleSewingOutCommand = new Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands.UpdateGarmentSampleSewingOutCommand()
            {
                RONo = "ro",
                Unit = new UnitDepartment(1, "code", "name"),
                UnitTo = new UnitDepartment(1, "code", "name"),
                Article = "",
                IsDifferentSize = true,
                Buyer = new Buyer(1, "code", "name"),
                SewingTo = "CUTTING",
                Comodity = new GarmentComodity(1, "como", "como"),
                SewingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleSewingOutItemValueObject>
                {
                    new GarmentSampleSewingOutItemValueObject
                    {
                        Id= sewingOutGuid,
                        Product = new Product(1, "code", "nama"),
                        Uom = new Uom(1, "uom"),
                        SewingInId= sewingOutGuid,
                        SewingInItemId=sewingOutGuid,
                        Color="Color",
                        Size=new SizeValueObject(1, "code"),
                        IsSave=true,

                        Quantity=1,
                        DesignColor= "color",
                        Details = new List<GarmentSampleSewingOutDetailValueObject>
                        {
                            new GarmentSampleSewingOutDetailValueObject
                            {
                                Id = sewingOutGuid,
                                Size=new SizeValueObject(1, "code"),
                                Uom = new Uom(1, "uom"),
                                Quantity=1
                            }
                        }
                    }
                },

            };
            UpdateGarmentSampleSewingOutCommand.SetIdentity(sewingOutGuid);
            GarmentComodityPrice GarmentSampleComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(UpdateGarmentSampleSewingOutCommand.Unit.Id),
                UpdateGarmentSampleSewingOutCommand.Unit.Code,
                UpdateGarmentSampleSewingOutCommand.Unit.Name,
                new GarmentComodityId(UpdateGarmentSampleSewingOutCommand.Comodity.Id),
                UpdateGarmentSampleSewingOutCommand.Comodity.Code,
                UpdateGarmentSampleSewingOutCommand.Comodity.Name,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    GarmentSampleComodity.GetReadModel()
                }.AsQueryable());

            _MockStorage
           .Setup(x => x.Save())
           .Verifiable();
            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSampleSewingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
        
    }
}