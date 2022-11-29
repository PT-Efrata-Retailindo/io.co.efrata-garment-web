using Barebone.Tests;
using Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;
using FluentAssertions;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconDeliveryLetterOuts
{
    public class RemoveGarmentSubconDeliveryLetterOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockSubconDeliveryLetterOutItemRepository;
        private readonly Mock<IGarmentSubconCuttingOutRepository> _mockSubconCuttingOutRepository;
        private readonly Mock<IGarmentServiceSubconCuttingRepository> _mockSubconCuttingRepository;
        private readonly Mock<IGarmentServiceSubconSewingRepository> _mockSubconSewingRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashRepository> _mockServiceSubconFabricWashRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelRepository> _mockServiceSubconShrinkagePanelRepository;
        private readonly Mock<IGarmentSubconContractRepository> _mockSubconContractRepository;

        public RemoveGarmentSubconDeliveryLetterOutCommandHandlerTests()
        {
            _mockSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();
            _mockSubconCuttingOutRepository = CreateMock<IGarmentSubconCuttingOutRepository>();
            _mockSubconCuttingRepository = CreateMock<IGarmentServiceSubconCuttingRepository>();
            _mockSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockServiceSubconFabricWashRepository = CreateMock<IGarmentServiceSubconFabricWashRepository>();
            _mockServiceSubconShrinkagePanelRepository = CreateMock<IGarmentServiceSubconShrinkagePanelRepository>();
            _mockSubconContractRepository = CreateMock<IGarmentSubconContractRepository>();

            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutItemRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingOutRepository);
            _MockStorage.SetupStorage(_mockSubconContractRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockSubconSewingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconFabricWashRepository);
            _MockStorage.SetupStorage(_mockServiceSubconShrinkagePanelRepository);
        }

        private RemoveGarmentSubconDeliveryLetterOutCommandHandler CreateRemoveGarmentSubconDeliveryLetterOutCommandHandler()
        {
            return new RemoveGarmentSubconDeliveryLetterOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_BB()
        {
            // Arrange
            Guid subconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid SubconContractGuid = Guid.NewGuid();
            RemoveGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateRemoveGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconDeliveryLetterOutCommand RemoveGarmentSubconDeliveryLetterOutCommand = new RemoveGarmentSubconDeliveryLetterOutCommand(subconDeliveryLetterOutGuid);

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
                {
                    new GarmentSubconDeliveryLetterOut(subconDeliveryLetterOutGuid,"","","SUBCON BAHAN BAKU",DateTimeOffset.Now,1,"","",1,"",false,"","",1,"",1,"").GetReadModel()
                }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,subconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",new Guid(),"","","",1,"")
                });

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_CT()
        {
            // Arrange
            Guid subconCuttingOutGuid = Guid.NewGuid();
            Guid subconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid SubconContractGuid = Guid.NewGuid();
            RemoveGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateRemoveGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconDeliveryLetterOutCommand RemoveGarmentSubconDeliveryLetterOutCommand = new RemoveGarmentSubconDeliveryLetterOutCommand(subconDeliveryLetterOutGuid);

            GarmentSubconCuttingOut garmentSubconCuttingOut = new GarmentSubconCuttingOut(subconCuttingOutGuid, "no", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, "ro", "", new GarmentComodityId(1), "", "", 1, 1, "", false);
            _mockSubconCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>
                {
                    garmentSubconCuttingOut.GetReadModel()
                }.AsQueryable());
            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
                {
                    new GarmentSubconDeliveryLetterOut(subconDeliveryLetterOutGuid,"","",SubconContractGuid,"","SUBCON CUTTING",DateTimeOffset.Now,1,"","",1,"",false,"").GetReadModel()
                }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,subconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconCuttingOutGuid,"","","")
                });

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _mockSubconCuttingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCuttingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCuttingOut>()));

            GarmentSubconContract garmentSubconContract = new GarmentSubconContract(
               SubconContractGuid, null, null, null, new SupplierId(1), "", "", null, null, null, 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "");

            _mockSubconContractRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconContractReadModel>()
                {
                    garmentSubconContract.GetReadModel()
                }.AsQueryable());
            _mockSubconContractRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContract>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContract>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_JS_CUTTING()
        {
            // Arrange
            Guid subconGuid = Guid.NewGuid();
            Guid subconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid SubconContractGuid = Guid.NewGuid();
            RemoveGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateRemoveGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconDeliveryLetterOutCommand RemoveGarmentSubconDeliveryLetterOutCommand = new RemoveGarmentSubconDeliveryLetterOutCommand(subconDeliveryLetterOutGuid);

            GarmentServiceSubconCutting garmentSubconCutting = new GarmentServiceSubconCutting(subconGuid, "no", "BORDIR", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, false, new BuyerId(1), "", "");

            _mockSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>
                {
                    garmentSubconCutting.GetReadModel()
                }.AsQueryable());

            _mockSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCutting>()));

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
                {
                    new GarmentSubconDeliveryLetterOut(subconDeliveryLetterOutGuid,"","",SubconContractGuid,"","SUBCON JASA",DateTimeOffset.Now,1,"","",1,"",false,"SUBCON JASA KOMPONEN").GetReadModel()
                }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,subconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconGuid,"","","")
                });

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            

            GarmentSubconContract garmentSubconContract = new GarmentSubconContract(
               SubconContractGuid, null, null, null, new SupplierId(1), "", "", null, null, null, 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "");

            _mockSubconContractRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconContractReadModel>()
                {
                    garmentSubconContract.GetReadModel()
                }.AsQueryable());
            _mockSubconContractRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContract>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContract>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_JS_SEWING()
        {
            // Arrange
            Guid subconGuid = Guid.NewGuid();
            Guid subconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid SubconContractGuid = Guid.NewGuid();
            RemoveGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateRemoveGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconDeliveryLetterOutCommand RemoveGarmentSubconDeliveryLetterOutCommand = new RemoveGarmentSubconDeliveryLetterOutCommand(subconDeliveryLetterOutGuid);

            GarmentServiceSubconSewing garmentSubconSewing = new GarmentServiceSubconSewing(subconGuid, "no", DateTimeOffset.Now, false, new BuyerId(1), "", "");

            _mockSubconSewingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingReadModel>
                {
                    garmentSubconSewing.GetReadModel()
                }.AsQueryable());

            _mockSubconSewingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewing>()));

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
                {
                    new GarmentSubconDeliveryLetterOut(subconDeliveryLetterOutGuid,"","",SubconContractGuid,"","SUBCON JASA",DateTimeOffset.Now,1,"","",1,"",false,"SUBCON JASA GARMENT WASH").GetReadModel()
                }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,subconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconGuid,"","","")
                });

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));



            GarmentSubconContract garmentSubconContract = new GarmentSubconContract(
               SubconContractGuid, null, null, null, new SupplierId(1), "", "", null, null, null, 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "");

            _mockSubconContractRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconContractReadModel>()
                {
                    garmentSubconContract.GetReadModel()
                }.AsQueryable());
            _mockSubconContractRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContract>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContract>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_JS_FABRIC()
        {
            // Arrange
            Guid subconGuid = Guid.NewGuid();
            Guid subconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid SubconContractGuid = Guid.NewGuid();
            RemoveGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateRemoveGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconDeliveryLetterOutCommand RemoveGarmentSubconDeliveryLetterOutCommand = new RemoveGarmentSubconDeliveryLetterOutCommand(subconDeliveryLetterOutGuid);

            GarmentServiceSubconFabricWash garmentSubconFabric = new GarmentServiceSubconFabricWash(subconGuid, "", DateTimeOffset.Now, false);

            _mockServiceSubconFabricWashRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashReadModel>
                {
                    garmentSubconFabric.GetReadModel()
                }.AsQueryable());

            _mockServiceSubconFabricWashRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconFabricWash>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconFabricWash>()));

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
                {
                    new GarmentSubconDeliveryLetterOut(subconDeliveryLetterOutGuid,"","",SubconContractGuid,"","SUBCON JASA",DateTimeOffset.Now,1,"","",1,"",false,"SUBCON JASA FABRIC WASH").GetReadModel()
                }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,subconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconGuid,"","","")
                });

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));



            GarmentSubconContract garmentSubconContract = new GarmentSubconContract(
               SubconContractGuid, null, null, null, new SupplierId(1), "", "", null, null, null, 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "");

            _mockSubconContractRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconContractReadModel>()
                {
                    garmentSubconContract.GetReadModel()
                }.AsQueryable());
            _mockSubconContractRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContract>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContract>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_JS_SHRINKAGE()
        {
            // Arrange
            Guid subconGuid = Guid.NewGuid();
            Guid subconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid SubconContractGuid = Guid.NewGuid();
            RemoveGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateRemoveGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconDeliveryLetterOutCommand RemoveGarmentSubconDeliveryLetterOutCommand = new RemoveGarmentSubconDeliveryLetterOutCommand(subconDeliveryLetterOutGuid);

            GarmentServiceSubconShrinkagePanel garmentSubconShrinkage = new GarmentServiceSubconShrinkagePanel(subconGuid, "", DateTimeOffset.Now, false);

            _mockServiceSubconShrinkagePanelRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelReadModel>
                {
                    garmentSubconShrinkage.GetReadModel()
                }.AsQueryable());

            _mockServiceSubconShrinkagePanelRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconShrinkagePanel>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconShrinkagePanel>()));

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
                {
                    new GarmentSubconDeliveryLetterOut(subconDeliveryLetterOutGuid,"","",SubconContractGuid,"","SUBCON JASA",DateTimeOffset.Now,1,"","",1,"",false,"SUBCON JASA SHRINKAGE PANEL").GetReadModel()
                }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,subconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconGuid,"","","")
                });

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));



            GarmentSubconContract garmentSubconContract = new GarmentSubconContract(
               SubconContractGuid, null, null, null, new SupplierId(1), "", "", null, null, null, 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "");

            _mockSubconContractRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconContractReadModel>()
                {
                    garmentSubconContract.GetReadModel()
                }.AsQueryable());
            _mockSubconContractRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContract>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContract>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
