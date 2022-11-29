using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentScrapSources.CommandHandler;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentScrapSources
{
    public class UpdateGarmentScrapSourceCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentScrapSourceRepository> _mockGarmentScrapSourceRepository;
        public UpdateGarmentScrapSourceCommandHandlerTest()
        {
            _mockGarmentScrapSourceRepository = CreateMock<IGarmentScrapSourceRepository>();
            _MockStorage.SetupStorage(_mockGarmentScrapSourceRepository);
        }


        private UpdateGarmentScrapSourceCommandHandler CreateUpdateGarmentScrapSourceCommandHandler()
        {
            return new UpdateGarmentScrapSourceCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid identity = Guid.NewGuid();
            UpdateGarmentScrapSourceCommandHandler unitUnderTest = CreateUpdateGarmentScrapSourceCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentScrapSourceCommand removeGarmentAvalComponentCommand = new RemoveGarmentScrapSourceCommand(identity);
            _mockGarmentScrapSourceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentScrapSourceReadModel>
                {
                    new GarmentScrapSource(identity,"code","name","description").GetReadModel()
                }.AsQueryable());

            _mockGarmentScrapSourceRepository
              .Setup(s => s.Update(It.IsAny<GarmentScrapSource>()))
              .Returns(Task.FromResult(It.IsAny<GarmentScrapSource>()));
            UpdateGarmentScrapSourceCommand updateGarmentScrapSourceCommand = new UpdateGarmentScrapSourceCommand()
            {
                Code = "codes",
                Name = "names",
                Description = "desss"
            };
            updateGarmentScrapSourceCommand.SetIdentity(identity);

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(updateGarmentScrapSourceCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
