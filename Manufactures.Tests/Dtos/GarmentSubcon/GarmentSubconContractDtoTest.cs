using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSubcon
{
    public class GarmentSubconContractDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSubconContractDto(new GarmentSubconContract(id,"type", "SubconContractNo", "no", new Domain.Shared.ValueObjects.SupplierId(1), "Code", "Name","type","No", "type",1, DateTimeOffset.Now, DateTimeOffset.Now,false, new BuyerId(1), "Code", "Name", "a", new UomId(1), "a", "a", DateTimeOffset.Now, 1));

            Assert.NotNull(dto.Supplier);

        }
    }
}
