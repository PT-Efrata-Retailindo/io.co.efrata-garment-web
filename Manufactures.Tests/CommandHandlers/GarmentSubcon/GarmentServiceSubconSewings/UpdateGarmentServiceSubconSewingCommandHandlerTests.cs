using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconSewings.CommandHandlers;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconSewings
{
    public class UpdateGarmentServiceSubconSewingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconSewingRepository> _mockServiceSubconSewingRepository;
        private readonly Mock<IGarmentServiceSubconSewingItemRepository> _mockServiceSubconSewingItemRepository;
        //private readonly Mock<IGarmentServiceSubconSewingDetailRepository> _mockServiceSubconSewingDetailRepository;
        //private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;

        public UpdateGarmentServiceSubconSewingCommandHandlerTests()
        {
            _mockServiceSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockServiceSubconSewingItemRepository = CreateMock<IGarmentServiceSubconSewingItemRepository>();
            // _mockServiceSubconSewingDetailRepository = CreateMock<IGarmentServiceSubconSewingDetailRepository>();
            //_mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconSewingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconSewingItemRepository);
            //_MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            // _MockStorage.SetupStorage(_mockServiceSubconSewingDetailRepository);
        }

        private UpdateGarmentServiceSubconSewingCommandHandler CreateUpdateGarmentServiceSubconSewingCommandHandler()
        {
            return new UpdateGarmentServiceSubconSewingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid serviceSubconSewingGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            Guid serviceSubconSewingItemGuid = Guid.NewGuid();
            UpdateGarmentServiceSubconSewingCommandHandler unitUnderTest = CreateUpdateGarmentServiceSubconSewingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentServiceSubconSewingCommand UpdateGarmentServiceSubconSewingCommand = new UpdateGarmentServiceSubconSewingCommand()
            {
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                Items = new List<GarmentServiceSubconSewingItemValueObject>
                {
                    new GarmentServiceSubconSewingItemValueObject
                    {
                        RONo = "RONo",
                        Article = "Article",
                        Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                        Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                        Details= new List<GarmentServiceSubconSewingDetailValueObject>
                        {
                            new GarmentServiceSubconSewingDetailValueObject
                            {
                                Product = new Product(1, "ProductCode", "ProductName"),
                                Uom = new Uom(1, "UomUnit"),
                                SewingInId= new Guid(),
                                SewingInItemId=sewingInItemGuid,
                                IsSave=true,
                                Quantity=1,
                                DesignColor= "ColorD",
                                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                            }
                        }

                    }
                },

            };

            UpdateGarmentServiceSubconSewingCommand.SetIdentity(serviceSubconSewingGuid);

            _mockServiceSubconSewingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingReadModel>()
                {
                    new GarmentServiceSubconSewingReadModel(serviceSubconSewingGuid)
                }.AsQueryable());

            //_mockServiceSubconSewingItemRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingItemReadModel, bool>>>()))
            //    .Returns(new List<GarmentServiceSubconSewingItem>()
            //    {
            //        new GarmentServiceSubconSewingItem(
            //            Guid.Empty,
            //            serviceSubconSewingGuid,
            //            null,
            //            null,
            //            new GarmentComodityId(1),
            //            null,
            //            null,
            //            new BuyerId(1),
            //            null,
            //            null)
            //    });
            //_mockServiceSubconSewingDetailRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingDetailReadModel, bool>>>()))
            //    .Returns(new List<GarmentServiceSubconSewingDetail>()
            //    {
            //        new GarmentServiceSubconSewingDetail(
            //            new Guid(),
            //            serviceSubconSewingItemGuid,
            //            Guid.Empty,
            //            Guid.Empty,
            //            new ProductId(1),
            //            null,
            //            null,
            //            null,
            //            1,
            //            new UomId(1),
            //            null,
            //            new UnitDepartmentId(1),
            //            null,
            //            null)
            //    });

            _mockServiceSubconSewingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewing>()));

            //_mockServiceSubconSewingItemRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewingItem>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewingItem>()));
            //_mockServiceSubconSewingDetailRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewingDetail>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewingDetail>()));

            //_mockGarmentPreparingRepository
            //    .Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>()))
            //    .Returns(true);

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentServiceSubconSewingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
