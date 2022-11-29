using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubconCuttingOuts.CommandHandlers;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Commands;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubconCuttingOuts
{
    public class PlaceGarmentSubconCuttingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconCuttingOutRepository> _mockSubconCuttingOutRepository;
        private readonly Mock<IGarmentSubconCuttingOutItemRepository> _mockSubconCuttingOutItemRepository;
        private readonly Mock<IGarmentSubconCuttingOutDetailRepository> _mockSubconCuttingOutDetailRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentSubconCuttingRepository> _mockSubconCuttingRepository;
        private readonly Mock<IGarmentSubconCuttingRelationRepository> _mockSubconCuttingRelationRepository;

        public PlaceGarmentSubconCuttingOutCommandHandlerTests()
        {
            _mockSubconCuttingOutRepository= CreateMock<IGarmentSubconCuttingOutRepository>();
            _mockSubconCuttingOutItemRepository = CreateMock<IGarmentSubconCuttingOutItemRepository>();
            _mockSubconCuttingOutDetailRepository = CreateMock<IGarmentSubconCuttingOutDetailRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockSubconCuttingRepository = CreateMock<IGarmentSubconCuttingRepository>();
            _mockSubconCuttingRelationRepository = CreateMock<IGarmentSubconCuttingRelationRepository>();

            _MockStorage.SetupStorage(_mockSubconCuttingOutRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingOutDetailRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingRelationRepository);
        }

        private PlaceGarmentSubconCuttingOutCommandHandler CreatePlaceGarmentSubconCuttingOutCommandHandler()
        {
            return new PlaceGarmentSubconCuttingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid cuttingInDetailGuid = Guid.NewGuid();
            Guid cuttingInGuid = Guid.NewGuid();
            PlaceGarmentSubconCuttingOutCommandHandler unitUnderTest = CreatePlaceGarmentSubconCuttingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSubconCuttingOutCommand placeGarmentSubconCuttingOutCommand = new PlaceGarmentSubconCuttingOutCommand()
            {
                RONo = "RONo",
                UnitFrom = new UnitDepartment(1, "UnitCode", "UnitName"),
                Comodity=new GarmentComodity(1, "ComoCode", "ComoName"),
                
                CuttingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSubconCuttingOutItemValueObject>
                {
                    new GarmentSubconCuttingOutItemValueObject
                    {
                        Product=new Product(1, "ProductCode", "ProductName"),
                        CuttingInDetailId=cuttingInDetailGuid,
                        IsSave=true,
                        CuttingInId=cuttingInGuid,
                        Details = new List<GarmentSubconCuttingOutDetailValueObject>
                        {
                            new GarmentSubconCuttingOutDetailValueObject
                            {
                                CuttingOutUom = new Uom(2, "PCS"),
                                CuttingOutQuantity=1,
                                Size= new SizeValueObject(1,"Size"),
                                Remark="asad"
                            }
                        }
                    }
                },

            };

            _mockSubconCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>().AsQueryable());
            _mockCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>
                {
                    new GarmentCuttingInDetailReadModel(cuttingInDetailGuid)
                }.AsQueryable());
            _mockSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconCuttingReadModel>
                {
                    new GarmentSubconCuttingReadModel(Guid.NewGuid())
                }.AsQueryable());

            _mockSubconCuttingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCuttingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCuttingOut>()));
            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCuttingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCuttingOutItem>()));
            _mockSubconCuttingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCuttingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCuttingOutDetail>()));
            _mockCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingInDetail>()));
            _mockSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCutting>()));
            _mockSubconCuttingRelationRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCuttingRelation>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCuttingRelation>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSubconCuttingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
