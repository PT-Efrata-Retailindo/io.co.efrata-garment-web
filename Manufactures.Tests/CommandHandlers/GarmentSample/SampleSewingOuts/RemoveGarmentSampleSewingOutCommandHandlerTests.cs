using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleSewingOuts.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ValueObjects;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleSewingOuts
{
    public class RemoveGarmentSampleSewingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleSewingOutRepository> _mockSewingOutRepository;
        private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockSewingOutItemRepository;
        private readonly Mock<IGarmentSampleSewingOutDetailRepository> _mockSewingOutDetailRepository;
        private readonly Mock<IGarmentSampleSewingInItemRepository> _mockSewingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInRepository> _mockCuttingInRepository;
        private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockCuttingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentSampleFinishingInRepository> _mockFinishingInRepository;
        private readonly Mock<IGarmentSampleFinishingInItemRepository> _mockFinishingInItemRepository;

        public RemoveGarmentSampleSewingOutCommandHandlerTests()
        {
            _mockSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();
            _mockSewingOutDetailRepository = CreateMock<IGarmentSampleSewingOutDetailRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();
            _mockCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockFinishingInRepository = CreateMock<IGarmentSampleFinishingInRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentSampleFinishingInItemRepository>();

            _MockStorage.SetupStorage(_mockSewingOutRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutDetailRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInRepository);
            _MockStorage.SetupStorage(_mockCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
        }
        private RemoveGarmentSampleSewingOutCommandHandler CreateRemoveGarmentSewingOutCommandHandler()
        {
            return new RemoveGarmentSampleSewingOutCommandHandler(_MockStorage.Object);
        }
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehaviorCuttingIsDiff()
        {

            Guid sewingOutGuid = Guid.NewGuid();
            RemoveGarmentSampleSewingOutCommandHandler unitUnderTest = CreateRemoveGarmentSewingOutCommandHandler();
            RemoveGarmentSampleSewingOutCommand removeGarmentSewingOutCommand = new RemoveGarmentSampleSewingOutCommand(sewingOutGuid);

            CancellationToken cancellationToken = CancellationToken.None;


            GarmentSampleCuttingIn garmentCuttingIn = new GarmentSampleCuttingIn(
                  sewingOutGuid, "", "", "", "", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, 9);
            GarmentSampleCuttingInItem garmentCuttingInItem = new GarmentSampleCuttingInItem(
                  sewingOutGuid, sewingOutGuid, sewingOutGuid, 1, "", sewingOutGuid, "");

            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInReadModel>()
                {
                    garmentCuttingIn.GetReadModel()
                }.AsQueryable());

            _mockCuttingInItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleCuttingInItemReadModel>()
               {
                   garmentCuttingInItem.GetReadModel()
               }
               .AsQueryable());
            GarmentSampleSewingInItem garmentSewingInItem = new GarmentSampleSewingInItem(
               sewingOutGuid, sewingOutGuid, sewingOutGuid, sewingOutGuid, Guid.Empty, Guid.Empty, new ProductId(1), "", "", "", new SizeId(1), "", 9, new UomId(1), "", "", 19, 19, 19);
            _mockSewingInItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleSewingInItemReadModel>()
               {
                   garmentSewingInItem.GetReadModel()
               }
               .AsQueryable());

            _mockCuttingInItemRepository
           .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInItemReadModel, bool>>>()))
           .Returns(new List<GarmentSampleCuttingInItem>()
           {
               new GarmentSampleCuttingInItem(sewingOutGuid,sewingOutGuid,sewingOutGuid,1,"",sewingOutGuid,"")
           });

            _mockCuttingInDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInDetailReadModel, bool>>>()))
         .Returns(new List<GarmentSampleCuttingInDetail>()
         {
                new GarmentSampleCuttingInDetail(sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,new ProductId(1),"","","","",10,new UomId(1),"",1,new UomId(1),"",1,12,12,2,"")
         });
            _mockCuttingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingIn>()));
            _mockCuttingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInItem>()));
            _mockCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInDetail>()));

            GarmentSampleSewingOut garmentSewingOut = new GarmentSampleSewingOut(
                sewingOutGuid, "No", new BuyerId(1), "", "", new UnitDepartmentId(1), "", "", "CUTTING", DateTimeOffset.Now, "", "", new UnitDepartmentId(1), "", "", new GarmentComodityId(1), "", "", true);

            _mockSewingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingOutReadModel>()
                {
                    garmentSewingOut.GetReadModel()
                }.AsQueryable());

            _mockSewingOutItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutItemReadModel, bool>>>()))
            .Returns(new List<GarmentSampleSewingOutItem>()
            {
               new GarmentSampleSewingOutItem(sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,new ProductId(1),"","","",new SizeId(1),"",1,new UomId(1),"uomUnit","",1,10,10)
            });

            _mockSewingOutDetailRepository
           .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutDetailReadModel, bool>>>()))
           .Returns(new List<GarmentSampleSewingOutDetail>()
           {
                new GarmentSampleSewingOutDetail(sewingOutGuid,sewingOutGuid,new SizeId(1),"",1,new UomId(1),"")
           });
            _mockSewingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOut>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutItem>()));
            _mockSewingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutDetail>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingInItem>()));

            _MockStorage
            .Setup(x => x.Save())
            .Verifiable();

            // Act
            RemoveGarmentSampleSewingOutCommand request = new RemoveGarmentSampleSewingOutCommand(sewingOutGuid);

            var result = await unitUnderTest.Handle(request, cancellationToken);


            result.Should().NotBeNull();
        }
        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehaviorCuttingIsNoDiff()
        //{

        //    Guid sewingOutGuid = Guid.NewGuid();
        //    RemoveGarmentSampleSewingOutCommandHandler unitUnderTest = CreateRemoveGarmentSewingOutCommandHandler();
        //    RemoveGarmentSampleSewingOutCommand removeGarmentSewingOutCommand = new RemoveGarmentSampleSewingOutCommand(sewingOutGuid);

        //    CancellationToken cancellationToken = CancellationToken.None;


        //    GarmentSampleCuttingIn garmentCuttingIn = new GarmentSampleCuttingIn(
        //          sewingOutGuid, "", "", "", "", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, 9);
        //    GarmentSampleCuttingInItem garmentCuttingInItem = new GarmentSampleCuttingInItem(
        //          sewingOutGuid, sewingOutGuid, sewingOutGuid, 1, "", sewingOutGuid, "");

        //    _mockCuttingInRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSampleCuttingInReadModel>()
        //        {
        //            garmentCuttingIn.GetReadModel()
        //        }.AsQueryable());

        //    _mockCuttingInItemRepository
        //       .Setup(s => s.Query)
        //       .Returns(new List<GarmentSampleCuttingInItemReadModel>()
        //       {
        //           garmentCuttingInItem.GetReadModel()
        //       }
        //       .AsQueryable());
        //    GarmentSampleSewingInItem garmentSewingInItem = new GarmentSampleSewingInItem(
        //       sewingOutGuid, sewingOutGuid, sewingOutGuid, sewingOutGuid, new ProductId(1), "", "", "", new SizeId(1), "", 9, new UomId(1), "", "", 19, 19, 19);
        //    _mockSewingInItemRepository
        //       .Setup(s => s.Query)
        //       .Returns(new List<GarmentSampleSewingInItemReadModel>()
        //       {
        //           garmentSewingInItem.GetReadModel()
        //       }
        //       .AsQueryable());

        //    _mockCuttingInItemRepository
        //   .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInItemReadModel, bool>>>()))
        //   .Returns(new List<GarmentSampleCuttingInItem>()
        //   {
        //       new GarmentSampleCuttingInItem(sewingOutGuid,sewingOutGuid,sewingOutGuid,1,"",sewingOutGuid,"")
        //   });

        //    _mockCuttingInDetailRepository
        //        .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInDetailReadModel, bool>>>()))
        // .Returns(new List<GarmentSampleCuttingInDetail>()
        // {
        //        new GarmentSampleCuttingInDetail(sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,new ProductId(1),"","","","",10,new UomId(1),"",1,new UomId(1),"",1,12,12,2,"")
        // });
        //    _mockCuttingInRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingIn>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingIn>()));
        //    _mockCuttingInItemRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInItem>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInItem>()));
        //    _mockCuttingInDetailRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInDetail>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInDetail>()));

        //    GarmentSampleSewingOut garmentSewingOut = new GarmentSampleSewingOut(
        //        sewingOutGuid, "No", new BuyerId(1), "", "", new UnitDepartmentId(1), "", "", "CUTTING", DateTimeOffset.Now, "", "", new UnitDepartmentId(1), "", "", new GarmentComodityId(1), "", "", false);

        //    _mockSewingOutRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSampleSewingOutReadModel>()
        //        {
        //            garmentSewingOut.GetReadModel()
        //        }.AsQueryable());

        //    _mockSewingOutItemRepository
        //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutItemReadModel, bool>>>()))
        //    .Returns(new List<GarmentSampleSewingOutItem>()
        //    {
        //       new GarmentSampleSewingOutItem(sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,new ProductId(1),"","","",new SizeId(1),"",1,new UomId(1),"uomUnit","",1,10,10)
        //    });

        //    _mockSewingOutDetailRepository
        //   .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutDetailReadModel, bool>>>()))
        //   .Returns(new List<GarmentSampleSewingOutDetail>()
        //   {
        //        new GarmentSampleSewingOutDetail(sewingOutGuid,sewingOutGuid,new SizeId(1),"",1,new UomId(1),"")
        //   });
        //    _mockSewingOutRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOut>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOut>()));
        //    _mockSewingOutItemRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutItem>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutItem>()));
        //    _mockSewingOutDetailRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutDetail>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutDetail>()));
        //    _mockSewingInItemRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleSewingInItem>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingInItem>()));

        //    _MockStorage
        //    .Setup(x => x.Save())
        //    .Verifiable();

        //    // Act
        //    RemoveGarmentSampleSewingOutCommand request = new RemoveGarmentSampleSewingOutCommand(sewingOutGuid);

        //    var result = await unitUnderTest.Handle(request, cancellationToken);


        //    result.Should().NotBeNull();
        //}
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehaviorFinishingIsDiff()
        {

            Guid sewingOutGuid = Guid.NewGuid();
            RemoveGarmentSampleSewingOutCommandHandler unitUnderTest = CreateRemoveGarmentSewingOutCommandHandler();
            RemoveGarmentSampleSewingOutCommand removeGarmentSewingOutCommand = new RemoveGarmentSampleSewingOutCommand(sewingOutGuid);

            CancellationToken cancellationToken = CancellationToken.None;

            GarmentSampleFinishingInItem garmentFinishingItem = new GarmentSampleFinishingInItem(
              sewingOutGuid, sewingOutGuid, sewingOutGuid, sewingOutGuid, sewingOutGuid, new SizeId(1), "", new ProductId(1), "", "", "", 11, 11, new UomId(1), "", "", 10, 10, 1);

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingInItemReadModel>()
                {
                      garmentFinishingItem.GetReadModel()
                }.AsQueryable());
            GarmentSampleFinishingIn garmentFinishing = new GarmentSampleFinishingIn(
               sewingOutGuid, "", "", new UnitDepartmentId(1), "", "", "", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, new GarmentComodityId(1), "", "", 1, "", "");

            _mockFinishingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingInReadModel>()
                {
                     garmentFinishing.GetReadModel()
                }.AsQueryable());


            GarmentSampleSewingInItem garmentSewingInItem = new GarmentSampleSewingInItem(
               sewingOutGuid, sewingOutGuid, sewingOutGuid, sewingOutGuid, Guid.Empty, Guid.Empty, new ProductId(1), "", "", "", new SizeId(1), "", 9, new UomId(1), "", "", 19, 19, 19);
            _mockSewingInItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleSewingInItemReadModel>()
               {
                   garmentSewingInItem.GetReadModel()
               }
               .AsQueryable());

            //_mockFinishingInItemRepository
            //.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingInItemReadModel, bool>>>()))
            //.Returns(new List<GarmentSampleFinishingInItem>()
            //{
            //     new GarmentSampleFinishingInItem(sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,new SizeId(1),"",new ProductId(1),"","","",11,11,new UomId(1),"","",10,10,1)
            //});


            _mockFinishingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingIn>()));

            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingInItem>()));


            GarmentSampleSewingOut garmentSewingOut = new GarmentSampleSewingOut(
                sewingOutGuid, "No", new BuyerId(1), "", "", new UnitDepartmentId(1), "", "", "FINISHING", DateTimeOffset.Now, "", "", new UnitDepartmentId(1), "", "", new GarmentComodityId(1), "", "", true);

            _mockSewingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingOutReadModel>()
                {
                    garmentSewingOut.GetReadModel()
                }.AsQueryable());



            _mockSewingOutItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutItemReadModel, bool>>>()))
            .Returns(new List<GarmentSampleSewingOutItem>()
            {
               new GarmentSampleSewingOutItem(sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,new ProductId(1),"","","",new SizeId(1),"",1,new UomId(1),"uomUnit","",1,10,10)
            });

            _mockSewingOutDetailRepository
           .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutDetailReadModel, bool>>>()))
           .Returns(new List<GarmentSampleSewingOutDetail>()
           {
                new GarmentSampleSewingOutDetail(sewingOutGuid,sewingOutGuid,new SizeId(1),"",1,new UomId(1),"")
           });
            _mockSewingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOut>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutItem>()));
            _mockSewingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutDetail>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingInItem>()));


            _MockStorage
            .Setup(x => x.Save())
            .Verifiable();

            // Act
            RemoveGarmentSampleSewingOutCommand request = new RemoveGarmentSampleSewingOutCommand(sewingOutGuid);

            var result = await unitUnderTest.Handle(request, cancellationToken);


            result.Should().NotBeNull();
        }
        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehaviorFinishingIsNotDiff()
        //{

        //    Guid sewingOutGuid = Guid.NewGuid();
        //    RemoveGarmentSampleSewingOutCommandHandler unitUnderTest = CreateRemoveGarmentSewingOutCommandHandler();
        //    RemoveGarmentSampleSewingOutCommand removeGarmentSewingOutCommand = new RemoveGarmentSampleSewingOutCommand(sewingOutGuid);

        //    CancellationToken cancellationToken = CancellationToken.None;

        //    GarmentSampleFinishingInItem garmentFinishingItem = new GarmentSampleFinishingInItem(
        //      sewingOutGuid, sewingOutGuid, sewingOutGuid, sewingOutGuid, sewingOutGuid, new SizeId(1), "", new ProductId(1), "", "", "", 11, 11, new UomId(1), "", "", 10, 10, 1);

        //    _mockFinishingInItemRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSampleFinishingInItemReadModel>()
        //        {
        //              garmentFinishingItem.GetReadModel()
        //        }.AsQueryable());
        //    GarmentSampleFinishingIn garmentFinishing = new GarmentSampleFinishingIn(
        //       sewingOutGuid, "", "", new UnitDepartmentId(1), "", "", "", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, new GarmentComodityId(1), "", "", 1, "", "");

        //    _mockFinishingInRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSampleFinishingInReadModel>()
        //        {
        //             garmentFinishing.GetReadModel()
        //        }.AsQueryable());


        //    GarmentSampleSewingInItem garmentSewingInItem = new GarmentSampleSewingInItem(
        //       sewingOutGuid, sewingOutGuid, sewingOutGuid, sewingOutGuid, new ProductId(1), "", "", "", new SizeId(1), "", 9, new UomId(1), "", "", 19, 19, 19);
        //    _mockSewingInItemRepository
        //       .Setup(s => s.Query)
        //       .Returns(new List<GarmentSampleSewingInItemReadModel>()
        //       {
        //           garmentSewingInItem.GetReadModel()
        //       }
        //       .AsQueryable());

        //    //_mockFinishingInItemRepository
        //    //.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingInItemReadModel, bool>>>()))
        //    //.Returns(new List<GarmentSampleFinishingInItem>()
        //    //{
        //    //     new GarmentSampleFinishingInItem(sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,new SizeId(1),"",new ProductId(1),"","","",11,11,new UomId(1),"","",10,10,1)
        //    //});


        //    _mockFinishingInRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingIn>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingIn>()));

        //    _mockFinishingInItemRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingInItem>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingInItem>()));


        //    GarmentSampleSewingOut garmentSewingOut = new GarmentSampleSewingOut(
        //        sewingOutGuid, "No", new BuyerId(1), "", "", new UnitDepartmentId(1), "", "", "FINISHING", DateTimeOffset.Now, "", "", new UnitDepartmentId(1), "", "", new GarmentComodityId(1), "", "", false);

        //    _mockSewingOutRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSampleSewingOutReadModel>()
        //        {
        //            garmentSewingOut.GetReadModel()
        //        }.AsQueryable());



        //    _mockSewingOutItemRepository
        //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutItemReadModel, bool>>>()))
        //    .Returns(new List<GarmentSampleSewingOutItem>()
        //    {
        //       new GarmentSampleSewingOutItem(sewingOutGuid,sewingOutGuid,sewingOutGuid,sewingOutGuid,new ProductId(1),"","","",new SizeId(1),"",1,new UomId(1),"uomUnit","",1,10,10)
        //    });

        //    _mockSewingOutDetailRepository
        //   .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutDetailReadModel, bool>>>()))
        //   .Returns(new List<GarmentSampleSewingOutDetail>()
        //   {
        //        new GarmentSampleSewingOutDetail(sewingOutGuid,sewingOutGuid,new SizeId(1),"",1,new UomId(1),"")
        //   });
        //    _mockSewingOutRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOut>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOut>()));
        //    _mockSewingOutItemRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutItem>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutItem>()));
        //    _mockSewingOutDetailRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutDetail>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutDetail>()));
        //    _mockSewingInItemRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSampleSewingInItem>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingInItem>()));


        //    _MockStorage
        //    .Setup(x => x.Save())
        //    .Verifiable();

        //    // Act
        //    RemoveGarmentSampleSewingOutCommand request = new RemoveGarmentSampleSewingOutCommand(sewingOutGuid);

        //    var result = await unitUnderTest.Handle(request, cancellationToken);


        //    result.Should().NotBeNull();

        //}
    }
}