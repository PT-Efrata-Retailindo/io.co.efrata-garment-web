using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentCuttingInDetailDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            var id = Guid.NewGuid();
            GarmentCuttingInDetailDto dto = new GarmentCuttingInDetailDto(new GarmentCuttingInDetail(new GarmentCuttingInDetailReadModel(id)));
            dto.LastModifiedBy = "LastModifiedBy";
            dto.CreatedBy = "CreatedBy";
            dto.Price = 1;
            dto.CutInItemId = id;
            dto.PreparingItemId = id;
            dto.Product = new Domain.Shared.ValueObjects.Product();
            dto.DesignColor = "DesignColor";
            dto.FabricType = "FabricType";
            dto.PreparingQuantity = 1;
            dto.PreparingRemainingQuantity = 1;
            dto.PreparingUom = new Domain.Shared.ValueObjects.Uom();
            dto.CuttingInUom = new Domain.Shared.ValueObjects.Uom();
            dto.RemainingQuantity = 1;
            dto.BasicPrice = 1;
            dto.Price = 1;
            dto.FC = 1;
            dto.Color = "Color";
            dto.CuttingInQuantity = 1;

            Assert.Equal(id, dto.Id);
            Assert.Equal(id, dto.CutInItemId);
            Assert.Equal(id, dto.PreparingItemId);
            Assert.Equal(id, dto.CutInItemId);
            Assert.Equal("DesignColor", dto.DesignColor);
            Assert.Equal("FabricType", dto.FabricType);
            Assert.Equal(1, dto.PreparingRemainingQuantity);
            Assert.Equal(1, dto.CuttingInQuantity);
            Assert.Equal("Color", dto.Color);
            Assert.Equal("LastModifiedBy", dto.LastModifiedBy);
            Assert.NotNull(dto.Product);
            Assert.NotNull(dto.PreparingUom);
            Assert.NotNull(dto.CuttingInUom);
            Assert.Equal(1, dto.RemainingQuantity);
            Assert.Equal(1, dto.BasicPrice);
            Assert.Equal(1, dto.FC);
            Assert.Equal(1, dto.Price);
            Assert.Equal(1, dto.PreparingQuantity);
            Assert.Equal(1, dto.PreparingRemainingQuantity);
            Assert.Equal(1, dto.CuttingInQuantity);

        }
        
    }
}
