using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentSubconCutting item = new GarmentSubconCutting(new GarmentSubconCuttingReadModel(id));
            item.SetQuantity(1);
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
