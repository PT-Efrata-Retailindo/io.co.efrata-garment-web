using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentComodityPrices.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Commands;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentComodityPrices.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentComodityPrices
{
    public class UpdateGarmentComodityPriceCommandHandlerTest : BaseCommandUnitTest
    {

        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;

        public UpdateGarmentComodityPriceCommandHandlerTest()
        {
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();

            _MockStorage.SetupStorage(_mockComodityPriceRepository);
        }

        private UpdateGarmentComodityPriceCommandHandler CreateUpdateGarmentComodityPriceCommandHandler()
        {
            return new UpdateGarmentComodityPriceCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            UpdateGarmentComodityPriceCommandHandler unitUnderTest = CreateUpdateGarmentComodityPriceCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentComodityPriceCommand request = new UpdateGarmentComodityPriceCommand()
            {
                Unit =new UnitDepartment()
                {
                    Id =1,
                    Code = "Code",
                    Name ="Name"
                },
                Date =DateTimeOffset.Now,
                Items =new List<GarmentComodityPriceItemValueObject>()
                {
                    new GarmentComodityPriceItemValueObject()
                    {
                        Id =id,
                        IsValid =true,
                        Comodity =new GarmentComodity()
                       {
                           Id =1,
                           Code ="Code",
                           Name ="Name"
                       },
                       Date =DateTimeOffset.Now,
                      NewPrice =2,
                      Price =2,
                      Unit =new UnitDepartment()
                      {
                          Id =1,
                          Code ="Code",
                          Name ="Name"
                      }
                      
                    }
                }
            };
            request.SetIdentity(id);

            GarmentComodityPrice garmentComodityPrice = new GarmentComodityPrice(id, true, DateTimeOffset.Now, new UnitDepartmentId(1), "unitCode", "unitName", new GarmentComodityId(1), "comodityCode", "comodityName", 1);
            _mockComodityPriceRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentComodityPriceReadModel>()
               {
                    garmentComodityPrice.GetReadModel()
               }.AsQueryable());

            _mockComodityPriceRepository
             .Setup(s => s.Update(It.IsAny<GarmentComodityPrice>()))
             .Returns(Task.FromResult(It.IsAny<GarmentComodityPrice>()));

            _MockStorage
               .Setup(x => x.Save())
               .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().BeGreaterThan(0);

        }
        }
}
