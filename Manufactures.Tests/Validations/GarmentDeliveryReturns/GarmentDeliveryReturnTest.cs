using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentDeliveryReturns
{
    public class GarmentDeliveryReturnTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentDeliveryReturn garmentDeliveryReturn = new GarmentDeliveryReturn(new GarmentDeliveryReturnReadModel(id));
            garmentDeliveryReturn.setArticle("Article");
            garmentDeliveryReturn.setUnitDOId(1);
            garmentDeliveryReturn.setStorageId(new StorageId(1));
            garmentDeliveryReturn.setIsUsed(true);
            Assert.NotNull(garmentDeliveryReturn);
        }
    }
}
