using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleAvalProducts.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleAvalProducts.CommandHandlers
{
    public class UpdateIsReceivedGarmentSampleAvalProductCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleAvalProductRepository> _mockGarmentSampleAvalProductRepository;
        private readonly Mock<IGarmentSampleAvalProductItemRepository> _mockGarmentSampleAvalProductItemRepository;

        public UpdateIsReceivedGarmentSampleAvalProductCommandHandlerTests()
        {
            _mockGarmentSampleAvalProductRepository = CreateMock<IGarmentSampleAvalProductRepository>();
            _mockGarmentSampleAvalProductItemRepository = CreateMock<IGarmentSampleAvalProductItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleAvalProductRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleAvalProductItemRepository);
        }

        private UpdateIsReceivedGarmentSampleAvalProductCommandHandler CreateUpdateIsReceivedGarmentSampleAvalProductCommandHandler()
        {
            return new UpdateIsReceivedGarmentSampleAvalProductCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            UpdateIsReceivedGarmentSampleAvalProductCommandHandler unitUnderTest = CreateUpdateIsReceivedGarmentSampleAvalProductCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateIsReceivedGarmentSampleAvalProductCommand request = new UpdateIsReceivedGarmentSampleAvalProductCommand(new List<string>() { id.ToString() }, true)
            {

            };

            _mockGarmentSampleAvalProductItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleAvalProductItemReadModel>() {
                    new GarmentSampleAvalProductItemReadModel(id)
               }.AsQueryable());

            _mockGarmentSampleAvalProductItemRepository
             .Setup(s => s.Update(It.IsAny<GarmentSampleAvalProductItem>()))
             .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalProductItem>()));

            _MockStorage
              .Setup(x => x.Save())
              .Verifiable();

            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().BeTrue();
        }
    }
}
