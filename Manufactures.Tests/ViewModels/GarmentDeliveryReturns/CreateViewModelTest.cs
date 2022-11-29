using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Manufactures.ViewModels.GarmentDeliveryReturns;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.ViewModels.GarmentDeliveryReturns
{
    public class CreateViewModelTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            var items = new List<GarmentDeliveryReturnItemValueObject>()
                {
                    new GarmentDeliveryReturnItemValueObject(id,id,1,1,"preparingItemId",new Product(),"designColor","roNo",1,new Uom(),id,1,1,true)
                };
            CreateViewModel viewModel = new CreateViewModel()
            {
                Article = "Article",
                DRNo = "DRNo",
                IsUsed = true,
                PreparingId = id.ToString(),
                RONo = "RONo",
                UnitCode = "UnitCode",
                UnitId = 1,
                UnitName = "UnitName",
                ReturnDate = date,
                ReturnType = "ReturnType",
                StorageCode = "StorageCode",
                StorageId = 1,
                StorageName = "StorageName",
                UENId = 1,
                UnitDOId = 1,
                UnitDONo = "UnitDONo",
                Items = items
            };


            Assert.Equal("Article", viewModel.Article);
            Assert.Equal("DRNo", viewModel.DRNo);
            Assert.True(viewModel.IsUsed);
            Assert.Equal(id.ToString(), viewModel.PreparingId);
            Assert.Equal("RONo", viewModel.RONo);
            Assert.Equal("UnitCode", viewModel.UnitCode);
            Assert.Equal(1, viewModel.UnitId);
            Assert.Equal("UnitName", viewModel.UnitName);
            Assert.Equal(date, viewModel.ReturnDate);
            Assert.Equal("ReturnType", viewModel.ReturnType);
            Assert.Equal("StorageCode", viewModel.StorageCode);
            Assert.Equal(1, viewModel.StorageId);
            Assert.Equal("StorageName", viewModel.StorageName);
            Assert.Equal(1, viewModel.UENId);
            Assert.Equal(1, viewModel.UnitDOId);
            Assert.Equal("UnitDONo", viewModel.UnitDONo);
            Assert.Equal(items, viewModel.Items);
        }
    }
}
