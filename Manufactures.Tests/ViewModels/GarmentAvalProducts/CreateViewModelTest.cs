using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Manufactures.ViewModels.GarmentAvalProducts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.ViewModels.GarmentAvalProducts
{
    public class CreateViewModelTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            var items = new List<GarmentAvalProductItemValueObject>()
                {
                    new GarmentAvalProductItemValueObject(id,id,new GarmentPreparingId("value"),new GarmentPreparingItemId("value"),new Product(),"designColor",1,new Uom())
                };
            CreateViewModel viewModel = new CreateViewModel()
            {
                Article = "Article",
                AvalDate = date,
                RONo = "RONo",
                UnitCode = "UnitCode",
                UnitId =1,
                UnitName = "UnitName",
                Items=items
            };

            Assert.Equal("Article", viewModel.Article);
            Assert.Equal(date, viewModel.AvalDate);
            Assert.Equal("RONo", viewModel.RONo);
            Assert.Equal("UnitCode", viewModel.UnitCode);
            Assert.Equal(1, viewModel.UnitId);
            Assert.Equal("UnitName", viewModel.UnitName);
            Assert.Equal(items,viewModel.Items);
        }
    }
}
