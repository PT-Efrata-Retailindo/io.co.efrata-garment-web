using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingOutTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentSubconCuttingOut item = new GarmentSubconCuttingOut(new GarmentCuttingOutReadModel(id));
           
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
