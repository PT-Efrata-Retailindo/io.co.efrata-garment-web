using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Manufactures.Dtos;
using Manufactures.ViewModels.GarmentPreparings;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.ViewModels.GarmentPreparings
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
                IsCuttingIn = true,
                ProcessDate = date,
                UENNo = "UENNo",
                RONo = "RONo",
                UnitCode = "UnitCode",
                UnitId = 1,
                UnitName = "UnitName",
                UENId = 1,
                Items = new List<GarmentPreparingItemValueObject>()
                {
                    new GarmentPreparingItemValueObject(id,1,new Product(),"designColor",1,new Uom(1,"uomUnit"),"fabricType",1,1,id,null)
                }
            };

            CreateViewModelMapper viewModelMapper = new CreateViewModelMapper();
            viewModelMapper.Map(viewModel);
        }

        [Fact]
        public void MapItem_Return_Success()
        {
            Guid id = Guid.NewGuid();
            GarmentPreparingItemDto dto = new GarmentPreparingItemDto(new GarmentPreparingItem(id,1,new ProductId(1),"productCode","productName","designColor",1,new UomId(1),"uomUnit","fabricType",1,1,id,null, "fasilitas"));
            CreateViewModelMapper viewModelMapper = new CreateViewModelMapper();
            viewModelMapper.MapItem(dto, id);
        }

    }
}
