using Barebone.Tests;
using Manufactures.Application.GarmentSample.SampleRequest.CommandHandler;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleRequests.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using System.Linq.Expressions;
using System.Linq;
using Manufactures.Domain.GarmentSample.SampleRequests;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleRequest
{
    public class UpdateGarmentSampleRequestCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleRequestRepository> _mockSampleRequestRepository;
        private readonly Mock<IGarmentSampleRequestProductRepository> _mockSampleRequestProductRepository;
        private readonly Mock<IGarmentSampleRequestSpecificationRepository> _mockSampleRequestSpecificationRepository;

        public UpdateGarmentSampleRequestCommandHandlerTests()
        {
            _mockSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();
            _mockSampleRequestProductRepository = CreateMock<IGarmentSampleRequestProductRepository>();
            _mockSampleRequestSpecificationRepository = CreateMock<IGarmentSampleRequestSpecificationRepository>();

            _MockStorage.SetupStorage(_mockSampleRequestRepository);
            _MockStorage.SetupStorage(_mockSampleRequestProductRepository);
            _MockStorage.SetupStorage(_mockSampleRequestSpecificationRepository);
        }
        private UpdateGarmentSampleRequestCommandHandler CreateUpdateGarmentSampleRequestCommandHandler()
        {
            return new UpdateGarmentSampleRequestCommandHandler(_MockStorage.Object, _MockServiceProvider.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInProductGuid = Guid.NewGuid();
            Guid SampleRequestGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentSampleRequestCommandHandler unitUnderTest = CreateUpdateGarmentSampleRequestCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSampleRequestCommand UpdateGarmentSampleRequestCommand = new UpdateGarmentSampleRequestCommand()
            {
                Date = DateTimeOffset.Now,
                SampleCategory = "Commercial Sample",
                Buyer = new Buyer
                {
                    Code = "test",
                    Id = 1,
                    Name = "test"
                },
                Comodity = new GarmentComodity
                {
                    Code = "test",
                    Id = 1,
                    Name = "test"
                },
                Section = new SectionValueObject
                {
                    Id = 1,
                    Code = "A"
                },
                SampleProducts = new List<GarmentSampleRequestProductValueObject>()
                {
                    new GarmentSampleRequestProductValueObject
                    {
                       Quantity=1,
                       Size=new SizeValueObject
                       {
                           Id=1,
                           Size="s"
                       }
                    }
                },
                SampleSpecifications = new List<GarmentSampleRequestSpecificationValueObject>()
                {
                    new GarmentSampleRequestSpecificationValueObject
                    {
                        Quantity=1,
                        Inventory="ACC",
                        Uom=new Uom
                        {
                            Id=1,
                            Unit="u"
                        }
                    }
                }

            };
            UpdateGarmentSampleRequestCommand.SetIdentity(SampleRequestGuid);

            _mockSampleRequestRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleRequestReadModel>()
                {
                    new GarmentSampleRequestReadModel(SampleRequestGuid)
                }.AsQueryable());


            _mockSampleRequestProductRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleRequestProductReadModel, bool>>>()))
                .Returns(new List<GarmentSampleRequestProduct>()
                {
                    new GarmentSampleRequestProduct(Guid.Empty,SampleRequestGuid,"a","a",new SizeId(2),"code","name",1,1)
                });

            _mockSampleRequestProductRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleRequestProduct>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleRequestProduct>()));

            _mockSampleRequestSpecificationRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleRequestSpecificationReadModel, bool>>>()))
                .Returns(new List<GarmentSampleRequestSpecification>()
                {
                    new GarmentSampleRequestSpecification(Guid.Empty,SampleRequestGuid,"a","a",1,"a",new UomId(2),"a",1),

                    new GarmentSampleRequestSpecification(Guid.Empty,SampleRequestGuid,"b","a",1,"a",new UomId(3),"a",2)
                });

            _mockSampleRequestSpecificationRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleRequestSpecification>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleRequestSpecification>()));

            _mockSampleRequestRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleRequest>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleRequest>()));


            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSampleRequestCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
