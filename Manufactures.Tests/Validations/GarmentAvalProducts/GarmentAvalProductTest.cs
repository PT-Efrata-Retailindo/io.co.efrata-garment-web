using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentAvalProducts
{
  public  class GarmentAvalProductTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentAvalProduct garment = new GarmentAvalProduct(new GarmentAvalProductReadModel(id));
            garment.SetArticle("Article");
            garment.SetAvalDate(DateTimeOffset.Now);
            garment.SetModified();
            garment.SetRONo("newRONo");
           
            Assert.NotNull(garment);
        }
    }
}
