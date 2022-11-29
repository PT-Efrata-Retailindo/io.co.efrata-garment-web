using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentCuttingOuts
{
    public class GarmentCuttingOutDetailTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentCuttingOutDetail detail = new GarmentCuttingOutDetail(new GarmentCuttingOutDetailReadModel(id));
            detail.SetCuttingOutQuantity(2);
            detail.SetColor("BLUE");
            detail.SetRemainingQuantity(4);
            detail.SetSizeId(new SizeId(1));
            detail.SetSizeName("NewName");
            detail.SetPrice(2);

            detail.Modify();
            Assert.NotNull(detail);
        }
    }
}
