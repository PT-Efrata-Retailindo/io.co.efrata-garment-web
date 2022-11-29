using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleFinishingIns.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleFinishingIns
{
    public class RemoveGarmentSampleFinishingInCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleFinishingInRepository> _mockFinishingInRepository;
        private readonly Mock<IGarmentSampleFinishingInItemRepository> _mockFinishingInItemRepository;
        private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockSewingOutItemRepository;

        public RemoveGarmentSampleFinishingInCommandHandlerTest()
        {
            _mockFinishingInRepository = CreateMock<IGarmentSampleFinishingInRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentSampleFinishingInItemRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();

            _MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
        }

        private RemoveGarmentSampleFinishingInCommandHandler CreateRemoveGarmentSampleFinishingInCommandHandler()
        {
            return new RemoveGarmentSampleFinishingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid loadingGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();
            Guid sewingOutGuid = Guid.NewGuid();
            RemoveGarmentSampleFinishingInCommandHandler unitUnderTest = CreateRemoveGarmentSampleFinishingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            Manufactures.Domain.GarmentSample.SampleFinishingIns.Commands.RemoveGarmentSampleFinishingInCommand RemoveGarmentSampleFinishingInCommand = new Manufactures.Domain.GarmentSample.SampleFinishingIns.Commands.RemoveGarmentSampleFinishingInCommand(loadingGuid);

            _mockFinishingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingInReadModel>()
                {
                    new GarmentSampleFinishingInReadModel(loadingGuid)
                }.AsQueryable());
            _mockFinishingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingInItem>()
                {
                    new GarmentSampleFinishingInItem(Guid.Empty, Guid.Empty,sewingOutItemGuid,Guid.Empty,Guid.Empty,new SizeId(1), null, new ProductId(1), null, null, null, 1,1,new UomId(1),null, null,1,1,1)
                });


            _mockSewingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingOutItemReadModel>
                {
                    new GarmentSampleSewingOutItemReadModel(sewingOutItemGuid)
                }.AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingIn>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingInItem>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSampleFinishingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
