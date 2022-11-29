using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentCuttingIns
{
    public class GarmentCuttingInTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentCuttingIn item = new GarmentCuttingIn(new GarmentCuttingInReadModel(id));
            item.SetFC(2);
            item.Modify();
            Assert.NotNull(item);
        }

    }
}
