using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingOutDetailTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentSubconCuttingOutDetail detail = new GarmentSubconCuttingOutDetail(new GarmentCuttingOutDetailReadModel(id));
            detail.SetCuttingOutQuantity(1);
            detail.SetRemainingQuantity(1);
            detail.SetColor("Color");
            detail.SetSizeId(new SizeId(1));
            detail.SetSizeName("SizeName");
            detail.Modify();
            Assert.NotNull(detail);
        }
    }
}
