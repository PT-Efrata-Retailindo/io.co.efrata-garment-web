using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Manufactures.ViewModels.GarmentPreparings;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.ViewModels.GarmentPreparings
{
    public class CreateViewModelTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            var items = new List<GarmentPreparingItemValueObject>()
                {
                    new GarmentPreparingItemValueObject(id,1,new Product(),"designColor",1,new Uom(1,"uomUnit"),"fabricType",1,1,id,"ro")
                };
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
                Items = items
            };


            Assert.Equal("Article", viewModel.Article);
            Assert.True(viewModel.IsCuttingIn);
            Assert.Equal(date, viewModel.ProcessDate);
            Assert.Equal("UENNo", viewModel.UENNo);
            Assert.Equal("RONo", viewModel.RONo);
            Assert.Equal("UnitCode", viewModel.UnitCode);
            Assert.Equal(1, viewModel.UnitId);
            Assert.Equal("UnitName", viewModel.UnitName);
            Assert.Equal(1, viewModel.UENId);
            Assert.Equal(items, viewModel.Items);
        }

    }
}
