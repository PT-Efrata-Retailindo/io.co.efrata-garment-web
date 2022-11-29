using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Manufactures.Dtos;
using Manufactures.ViewModels.GarmentAvalProducts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.ViewModels.GarmentAvalProducts
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
                AvalDate = date,
                RONo = "RONo",
                UnitCode = "UnitCode",
                UnitId = 1,
                UnitName = "UnitName",
                Items = new List<GarmentAvalProductItemValueObject>()
                {
                     new GarmentAvalProductItemValueObject(id,id,new GarmentPreparingId("value"),new GarmentPreparingItemId("value"),new Product(),"designColor",1,new Uom())
                }
            };

            CreateViewModelMapper viewModelMapper = new CreateViewModelMapper();
            viewModelMapper.Map(viewModel);
        }

        [Fact]
        public void MapItem_Return_Success()
        {
            Guid id = Guid.NewGuid();
            GarmentAvalProductItemDto dto = new GarmentAvalProductItemDto(new GarmentAvalProductItem(id, id, new GarmentPreparingId("value"), new GarmentPreparingItemId("value"), new ProductId(1), "productCode", "productName", "designColor", 1, new UomId(1), "uomUnit", 1, true));
            CreateViewModelMapper viewModelMapper = new CreateViewModelMapper();
            viewModelMapper.MapItem(dto, id);
        }
    }
}
