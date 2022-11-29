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

namespace Manufactures.Tests.CommandHandlers.GarmentSubconCuttingOuts
{
    public class RemoveGarmentSubconCuttingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconCuttingOutRepository> _mockSubconCuttingOutRepository;
        private readonly Mock<IGarmentSubconCuttingOutItemRepository> _mockSubconCuttingOutItemRepository;
        private readonly Mock<IGarmentSubconCuttingOutDetailRepository> _mockSubconCuttingOutDetailRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentSubconCuttingRepository> _mockSubconCuttingRepository;
        private readonly Mock<IGarmentSubconCuttingRelationRepository> _mockSubconCuttingRelationRepository;

        public RemoveGarmentSubconCuttingOutCommandHandlerTests()
        {
            _mockSubconCuttingOutRepository = CreateMock<IGarmentSubconCuttingOutRepository>();
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

        private RemoveGarmentSubconCuttingOutCommandHandler CreateRemoveGarmentSubconCuttingOutCommandHandler()
        {
            return new RemoveGarmentSubconCuttingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid cuttingInGuid = Guid.NewGuid();
            Guid cuttingOutGuid= Guid.NewGuid();
            Guid cuttingInDetailGuid = Guid.NewGuid();
            RemoveGarmentSubconCuttingOutCommandHandler unitUnderTest = CreateRemoveGarmentSubconCuttingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSubconCuttingOutCommand RemoveGarmentSubconCuttingOutCommand = new RemoveGarmentSubconCuttingOutCommand(cuttingInGuid);

            _mockSubconCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>()
                {
                    new GarmentCuttingOutReadModel(cuttingInGuid)
                }.AsQueryable());
            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCuttingOutItem>()
                {
                    new GarmentSubconCuttingOutItem(Guid.Empty, cuttingInGuid, cuttingInDetailGuid, cuttingOutGuid,new ProductId(1), null, null, null, 0)
                });

            _mockSubconCuttingOutDetailRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutDetailReadModel, bool>>>()))
               .Returns(new List<GarmentSubconCuttingOutDetail>()
               {
                    new GarmentSubconCuttingOutDetail(Guid.Empty, Guid.Empty, new SizeId(1), null, null, 0, 0, new UomId(1), null, 0,0,"asa")
               });

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
					new GarmentSubconCutting(new Guid(),"", new SizeId(1),"",0,new ProductId(1),"","",new GarmentComodityId(0),"","","","asa",0).GetReadModel()
				}.AsQueryable());

            _mockSubconCuttingRelationRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSubconCuttingRelationReadModel>
				{
					new GarmentSubconCuttingRelation(new Guid(), new Guid(), new Guid()).GetReadModel()
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
            var result = await unitUnderTest.Handle(RemoveGarmentSubconCuttingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
