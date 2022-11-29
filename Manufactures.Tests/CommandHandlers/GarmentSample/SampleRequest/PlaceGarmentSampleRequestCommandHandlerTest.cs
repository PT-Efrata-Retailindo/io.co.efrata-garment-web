using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleRequest.CommandHandler;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleRequest
{
    public class PlaceGarmentSampleRequestCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleRequestRepository> _mockSampleRequestRepository;
        private readonly Mock<IGarmentSampleRequestProductRepository> _mockSampleRequestProductRepository;
        private readonly Mock<IGarmentSampleRequestSpecificationRepository> _mockSampleRequestSpecificationRepository;
        public PlaceGarmentSampleRequestCommandHandlerTest()
        {
            _mockSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();
            _mockSampleRequestProductRepository = CreateMock<IGarmentSampleRequestProductRepository>();
            _mockSampleRequestSpecificationRepository = CreateMock<IGarmentSampleRequestSpecificationRepository>();

            _MockStorage.SetupStorage(_mockSampleRequestRepository);
            _MockStorage.SetupStorage(_mockSampleRequestProductRepository);
            _MockStorage.SetupStorage(_mockSampleRequestSpecificationRepository);
        }
        private PlaceGarmentSampleRequestCommandHandler CreatePlaceGarmentSampleRequestCommandHandler()
        {
            return new PlaceGarmentSampleRequestCommandHandler(_MockStorage.Object, _MockServiceProvider.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SampleRequestGuid = Guid.NewGuid();
            PlaceGarmentSampleRequestCommandHandler unitUnderTest = CreatePlaceGarmentSampleRequestCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleRequestCommand placeGarmentSampleRequestCommand = new PlaceGarmentSampleRequestCommand()
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
                Section= new SectionValueObject
                {
                    Id=1,
                    Code="A"
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
                SampleSpecifications= new List<GarmentSampleRequestSpecificationValueObject>()
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
            _mockSampleRequestRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleRequestReadModel>().AsQueryable());
            _mockSampleRequestRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleRequest>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleRequest>()));

            _mockSampleRequestProductRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleRequestProduct>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleRequestProduct>()));

            _mockSampleRequestSpecificationRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleRequestSpecification>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleRequestSpecification>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSampleRequestCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}