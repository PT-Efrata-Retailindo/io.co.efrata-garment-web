using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentScrapClassifications.CommandHandler;
using Manufactures.Domain.GarmentScrapClassifications;
using Manufactures.Domain.GarmentScrapClassifications.Commands;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentScrapClassifications
{
	public class PlaceGarmentScrapClassificationCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentScrapClassificationRepository> _mockGarmentScrapClassificationRepository;
		public PlaceGarmentScrapClassificationCommandHandlerTest()
		{
			_mockGarmentScrapClassificationRepository = CreateMock<IGarmentScrapClassificationRepository>();
			_MockStorage.SetupStorage(_mockGarmentScrapClassificationRepository);
		}


		private PlaceGarmentScrapClassificationCommandHandler CreatePlaceGarmentScrapClassificationCommandHandler()
		{
			return new PlaceGarmentScrapClassificationCommandHandler(_MockStorage.Object);
		}
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			PlaceGarmentScrapClassificationCommandHandler unitUnderTest = CreatePlaceGarmentScrapClassificationCommandHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guid = Guid.NewGuid();

			PlaceGarmentScrapClassificationCommand placeGarmentScrapClassificationCommand = new PlaceGarmentScrapClassificationCommand()
			{
				Code="code",
				Name="name",
				Description="desc"
			};

			_mockGarmentScrapClassificationRepository
				.Setup(s => s.Update(It.IsAny<GarmentScrapClassification>()))
				.Returns(Task.FromResult(It.IsAny<GarmentScrapClassification>()));

			_MockStorage
				.Setup(x => x.Save())
				.Verifiable();

			// Act
			var result = await unitUnderTest.Handle(placeGarmentScrapClassificationCommand, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
