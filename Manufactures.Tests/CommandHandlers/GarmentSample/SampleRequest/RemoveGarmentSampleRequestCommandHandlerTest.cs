using Barebone.Tests;
using Manufactures.Application.GarmentSample.SampleRequest.CommandHandler;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleRequest
{
    public class RemoveGarmentSampleRequestCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleRequestRepository> _mockSampleRequestRepository;
        private readonly Mock<IGarmentSampleRequestProductRepository> _mockSampleRequestProductRepository;
        private readonly Mock<IGarmentSampleRequestSpecificationRepository> _mockSampleRequestSpecificationRepository;
        public RemoveGarmentSampleRequestCommandHandlerTest()
        {
            _mockSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();
            _mockSampleRequestProductRepository = CreateMock<IGarmentSampleRequestProductRepository>();
            _mockSampleRequestSpecificationRepository = CreateMock<IGarmentSampleRequestSpecificationRepository>();

            _MockStorage.SetupStorage(_mockSampleRequestRepository);
            _MockStorage.SetupStorage(_mockSampleRequestProductRepository);
            _MockStorage.SetupStorage(_mockSampleRequestSpecificationRepository);
        }
        private RemoveGarmentSampleRequestCommandHandler CreateRemoveGarmentSampleRequestCommandHandler()
        {
            return new RemoveGarmentSampleRequestCommandHandler(_MockStorage.Object, _MockServiceProvider.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SampleRequestGuid = Guid.NewGuid();
            Guid SampleRequestItemGuid = Guid.NewGuid();
            RemoveGarmentSampleRequestCommandHandler unitUnderTest = CreateRemoveGarmentSampleRequestCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSampleRequestCommand RemoveGarmentSampleRequestCommand = new RemoveGarmentSampleRequestCommand(SampleRequestGuid);

            GarmentSampleRequest garmentSampleRequest = new GarmentSampleRequest(
                SampleRequestGuid,null,null,null,null, DateTimeOffset.Now, new BuyerId(1), "", "", new GarmentComodityId(1),null,null,"","", DateTimeOffset.Now,"","","",
                false,false, DateTimeOffset.Now, null, false, DateTimeOffset.Now, null, false, null, null, null, null,  null, null, new SectionId(1), null);

            _mockSampleRequestRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleRequestReadModel>()
                {
                    garmentSampleRequest.GetReadModel()
                }.AsQueryable());

            _mockSampleRequestRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleRequest>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleRequest>()));

            _mockSampleRequestProductRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleRequestProductReadModel, bool>>>()))
               .Returns(new List<GarmentSampleRequestProduct>()
               {
                    new GarmentSampleRequestProduct(Guid.Empty,SampleRequestGuid,null,null,new SizeId(1),"code","name",1,1)
               });
            _mockSampleRequestProductRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleRequestProduct>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleRequestProduct>()));

            _mockSampleRequestSpecificationRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleRequestSpecificationReadModel, bool>>>()))
                .Returns(new List<GarmentSampleRequestSpecification>()
                {
                    new GarmentSampleRequestSpecification(Guid.Empty,SampleRequestGuid,null,null,1,null,new UomId(1),null,1)
                });

            _mockSampleRequestSpecificationRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleRequestSpecification>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleRequestSpecification>()));
            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSampleRequestCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
