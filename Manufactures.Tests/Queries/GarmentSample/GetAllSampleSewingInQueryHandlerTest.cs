//using Barebone.Tests;
//using FluentAssertions;
//using Manufactures.Application.GarmentSample.SampleSewingIns.Queries;
//using Manufactures.Domain.GarmentSample.SampleSewingIns;
//using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
//using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
//using Manufactures.Domain.Shared.ValueObjects;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace Manufactures.Tests.Queries.GarmentSample
//{
//    public class GetAllSampleSewingInQueryHandlerTest : BaseCommandUnitTest
//    {
//        private readonly Mock<IGarmentSampleSewingInRepository> _mockGarmentSampleSewingInRepository;
//        private readonly Mock<IGarmentSampleSewingInItemRepository> _mockGarmentSampleSewingInItemRepository;

//        public GetAllSampleSewingInQueryHandlerTest()
//        {
//            _mockGarmentSampleSewingInRepository = CreateMock<IGarmentSampleSewingInRepository>();
//            _mockGarmentSampleSewingInItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();

//            _MockStorage.SetupStorage(_mockGarmentSampleSewingInRepository);
//            _MockStorage.SetupStorage(_mockGarmentSampleSewingInItemRepository);
//        }

//        private GetAllSampleSewingInQueryHandler CreateGetAllSewingInQueryHandler()
//        {
//            return new GetAllSampleSewingInQueryHandler(_MockStorage.Object);
//        }

//        [Fact]
//        public async Task Handle_GetAllSewingIn_Success()
//        {
//            GetAllSampleSewingInQueryHandler unitUnderTest = CreateGetAllSewingInQueryHandler();
//            CancellationToken cancellationToken = CancellationToken.None;

//            Guid guidSewingIn = Guid.NewGuid();
//            Guid guidSewingInItem = Guid.NewGuid();
//            Guid guidSewingInDetail = Guid.NewGuid();

//            GarmentSampleSewingInReadModel garmentSewingIn = new GarmentSampleSewingIn(guidSewingIn, "cutOutNo", "CUTTING", 
//                Guid.NewGuid(),"cutOutNo", new UnitDepartmentId(1), "unitFromCode", "unitFromName"
//                , new UnitDepartmentId(1), "unitCode", "unitName","roNo","art", new GarmentComodityId(1), "comodityCode", 
//                "comodityName", DateTimeOffset.Now).GetReadModel();
//            GarmentSampleSewingInItemReadModel garmentSewingInItem = new GarmentSampleSewingInItem(guidSewingInItem, guidSewingIn,
//                Guid.NewGuid(), guidSewingIn, new ProductId(1), "productCode", "productName", "designColor", new SizeId(1),"name",
//                10, new UomId(1),"unit","color", 10,0,0).GetReadModel();
//            garmentSewingIn.GarmentSampleSewingInItem.Add(new GarmentSampleSewingInItem(guidSewingInItem, guidSewingIn,
//                Guid.NewGuid(), guidSewingIn, new ProductId(1), "productCode", "productName", "designColor", new SizeId(1), "name",
//                10, new UomId(1), "unit", "color", 10, 0, 0).GetReadModel());

//            _mockGarmentSampleSewingInRepository.Setup(s => s.Query).Returns(new List<GarmentSampleSewingInReadModel>
//                {
//                    garmentSewingIn
//                }.AsQueryable());
//            _mockGarmentSampleSewingInItemRepository.Setup(s => s.Query).Returns(new List<GarmentSampleSewingInItemReadModel>
//                {
//                    garmentSewingInItem
//                }.AsQueryable());
            
//            GetAllSampleSewingInQuery query = new GetAllSampleSewingInQuery(1, 25, "{}", "rONo", "{}");

//            // Act
//            var result = await unitUnderTest.Handle(query, cancellationToken);

//            // Assert
//            result.Should().NotBeNull();

//        }
//    }
//}

