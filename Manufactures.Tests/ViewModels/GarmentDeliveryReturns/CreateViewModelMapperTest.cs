using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Manufactures.Dtos;
using Manufactures.ViewModels.GarmentDeliveryReturns;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.ViewModels.GarmentDeliveryReturns
{
    public class CreateViewModelMapperTest
    {
        [Fact]
        public void Map_Return_Success()
        {
            Guid id = Guid.NewGuid();
            var date = DateTimeOffset.Now;
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
                Items = new List<GarmentDeliveryReturnItemValueObject>()
                {
                    new GarmentDeliveryReturnItemValueObject()
                }
            };

            CreateViewModelMapper viewModelMapper = new CreateViewModelMapper();
            viewModelMapper.Map(viewModel);
        }

        [Fact]
        public void MapItem_Return_Success()
        {
            Guid id = Guid.NewGuid();
            GarmentDeliveryReturnItemDto dto = new GarmentDeliveryReturnItemDto(new GarmentDeliveryReturnItem(id,id,1,1,"preparingItemId",new ProductId(1),"productCode", "productName","designColor","roNo",1,new UomId(1),"uomUnit"));
            CreateViewModelMapper viewModelMapper = new CreateViewModelMapper();
            viewModelMapper.MapItem(dto, id);
        }
    }
}
