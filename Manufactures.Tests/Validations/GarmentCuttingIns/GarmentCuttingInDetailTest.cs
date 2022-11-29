using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentCuttingIns
{
    public class GarmentCuttingInDetailTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentCuttingInDetail detail = new GarmentCuttingInDetail(new GarmentCuttingInDetailReadModel(id));
            detail.SetPreparingQuantity(1);
            detail.SetCuttingInQuantity(2);

            detail.Modify();
            Assert.NotNull(detail);
        }

    }
}
