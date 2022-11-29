using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconContactReport;
using Barebone.Tests;
using Manufactures.Application.GarmentSubcon.Queries.GarmentRealizationSubconReport;
using System.Threading.Tasks;
using System.Threading;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using FluentAssertions;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentRealizationSubcon
{
    public class GarmentRealizationSubconQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconCustomsInRepository> _mockgarmentSubconCustomsInRepository;
        private readonly Mock<IGarmentSubconCustomsInItemRepository> _mockgarmentSubconCustomsInItemRepository;
        private readonly Mock<IGarmentSubconCustomsOutRepository> _mockgarmentSubconCustomsOutRepository;
        private readonly Mock<IGarmentSubconCustomsOutItemRepository> _mockgarmentSubconCustomsOutItemRepository;
        private readonly Mock<IGarmentSubconContractRepository> _mockgarmentSubconContractRepository;
        private readonly Mock<IGarmentSubconContractItemRepository> _mockgarmentSubconContractItemRepository;

        private Mock<IServiceProvider> serviceProviderMock;

        public GarmentRealizationSubconQueryHandlerTest()
        {
            _mockgarmentSubconCustomsInRepository = CreateMock<IGarmentSubconCustomsInRepository>();
            _mockgarmentSubconCustomsInItemRepository = CreateMock<IGarmentSubconCustomsInItemRepository>();
            _mockgarmentSubconCustomsOutRepository = CreateMock<IGarmentSubconCustomsOutRepository>();
            _mockgarmentSubconCustomsOutItemRepository = CreateMock<IGarmentSubconCustomsOutItemRepository>();
            _mockgarmentSubconContractRepository = CreateMock<IGarmentSubconContractRepository>();
            _mockgarmentSubconContractItemRepository = CreateMock<IGarmentSubconContractItemRepository>();

            _MockStorage.SetupStorage(_mockgarmentSubconCustomsInRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconCustomsInItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconCustomsOutRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconCustomsOutItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconContractRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconContractItemRepository);

            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GarmentRealizationSubconReportQueryHandler CreateGetPrepareTraceableQueryHandler()
        {
            return new GarmentRealizationSubconReportQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior()
        //{
        //    GarmentRealizationSubconReportQueryHandler unitUnderTest = CreateGetPrepareTraceableQueryHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;


        //    Guid guidSubconCustomsIn = Guid.NewGuid();
        //    Guid guidSubconCustomsInItem = Guid.NewGuid();
        //    Guid guidSubconCustomsOut = Guid.NewGuid();
        //    Guid guidSubconCustomsOutItem = Guid.NewGuid();
        //    Guid guidSubconContract = Guid.NewGuid();
        //    Guid guidSubconContractItem = Guid.NewGuid();


        //    GarmentRealizationSubconReportQuery getMonitoring = new GarmentRealizationSubconReportQuery(1,25,"","subconcontract", "token");


        //    _mockgarmentSubconContractRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconContractReadModel>
        //        {
        //                new GarmentSubconContract(guidSubconContract, "contractType", "subconcontract", "agreementNo", new SupplierId (1), "supplierCode", "supplierName", "jobType", "bPJNo", "finishedGoodType", 12, DateTimeOffset.Now, DateTimeOffset.Now, true, new BuyerId(1), "buyerCode", "buyerName", "subconCategory", new UomId(1), "uomUnit", "sKEPNo", DateTimeOffset.Now).GetReadModel()

        //        }.AsQueryable());

        //    _mockgarmentSubconContractItemRepository
        //         .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconContractItemReadModel>
        //        {
        //            new GarmentSubconContractItem(guidSubconContractItem, guidSubconContract,new ProductId(1), "productCode", "productName", 21,new UomId(1), "uomUnit").GetReadModel()
        //        }.AsQueryable());

        //    _mockgarmentSubconCustomsInRepository
        //         .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconCustomsInReadModel>
        //        {
        //            new GarmentSubconCustomsIn(guidSubconCustomsIn, "bcNo", DateTimeOffset.Now, "bcType", "subconType", guidSubconContract, "subconcontract", new SupplierId(1), "supplierCode", "supplierName", "remark", true, "subconCategory").GetReadModel()
        //        }.AsQueryable());

        //    _mockgarmentSubconCustomsInItemRepository
        //         .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconCustomsInItemReadModel>
        //        {
        //            new GarmentSubconCustomsInItem(guidSubconCustomsInItem, guidSubconCustomsIn, new SupplierId(1), "supplierCode", "supplierName", 1, "doNo", 2).GetReadModel()
        //        }.AsQueryable());

        //    _mockgarmentSubconCustomsOutRepository
        //         .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconCustomsOutReadModel>
        //        {
        //            new GarmentSubconCustomsOut(guidSubconCustomsOut, "customsOutNo", DateTimeOffset.Now, "customsOutType", "subconType", guidSubconContract, "subconcontract", new SupplierId(1), "supplierCode", "supplierName", "remark", "subconCategory").GetReadModel()
        //        }.AsQueryable());

        //    _mockgarmentSubconCustomsOutItemRepository
        //         .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconCustomsOutItemReadModel>
        //        {
        //            new GarmentSubconCustomsOutItem(guidSubconCustomsOutItem, guidSubconCustomsOut, "subconDLOutNo", Guid.NewGuid(), 2).GetReadModel()
        //        }.AsQueryable());


        //    var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();

        //}
    }
}
