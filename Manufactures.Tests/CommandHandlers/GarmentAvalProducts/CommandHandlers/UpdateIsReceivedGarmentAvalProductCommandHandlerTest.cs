using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalProducts.CommandHandlers;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentAvalProducts.CommandHandlers
{
    public class UpdateIsReceivedGarmentAvalProductCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentAvalProductRepository> _mockGarmentAvalProductRepository;
        private readonly Mock<IGarmentAvalProductItemRepository> _mockGarmentAvalProductItemRepository;
        
        public UpdateIsReceivedGarmentAvalProductCommandHandlerTest()
        {
            _mockGarmentAvalProductRepository = CreateMock<IGarmentAvalProductRepository>();
            _mockGarmentAvalProductItemRepository = CreateMock<IGarmentAvalProductItemRepository>();
           
            _MockStorage.SetupStorage(_mockGarmentAvalProductRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalProductItemRepository);
        }


        private UpdateIsReceivedGarmentAvalProductCommandHandler CreateUpdateIsReceivedGarmentAvalProductCommandHandler()
        {
            return new UpdateIsReceivedGarmentAvalProductCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            UpdateIsReceivedGarmentAvalProductCommandHandler unitUnderTest = CreateUpdateIsReceivedGarmentAvalProductCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateIsReceivedGarmentAvalProductCommand request = new UpdateIsReceivedGarmentAvalProductCommand(new List<string>() { id.ToString()},true)
            {
                
            };

            _mockGarmentAvalProductItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentAvalProductItemReadModel>() { 
                    new GarmentAvalProductItemReadModel(id)
               }.AsQueryable());

            _mockGarmentAvalProductItemRepository
             .Setup(s => s.Update(It.IsAny<GarmentAvalProductItem>()))
             .Returns(Task.FromResult(It.IsAny<GarmentAvalProductItem>()));

            _MockStorage
              .Setup(x => x.Save())
              .Verifiable();

            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().BeTrue();
        }


        }
}
