using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentCuttingInListDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            var id = Guid.NewGuid();
            GarmentCuttingInListDto dto = new GarmentCuttingInListDto(new GarmentCuttingIn(new GarmentCuttingInReadModel(id)));
            dto.CutInNo = "CutInNo";
            dto.CreatedBy = "CreatedBy";
            dto.CuttingType = "CuttingType";
            dto.CuttingFrom = "CuttingFrom";
            dto.RONo = "RONo";
            dto.Article = "Article";
            dto.Unit = new Domain.Shared.ValueObjects.UnitDepartment();
            dto.CuttingInDate = DateTimeOffset.Now;
            dto.FC = 1;
            dto.TotalCuttingInQuantity =1;
            dto.UENNos = new List<string>();
            dto.Products = new List<string>();

            Assert.True(DateTimeOffset.MinValue < dto.CuttingInDate);
            Assert.Equal("CutInNo", dto.CutInNo);
            Assert.Equal("CuttingType", dto.CuttingType);
            Assert.Equal("CuttingFrom", dto.CuttingFrom);
            Assert.Equal("RONo", dto.RONo);
            Assert.NotNull(dto.Unit);
            Assert.NotNull(dto.CreatedBy);
            Assert.NotNull(dto.UENNos);
            Assert.NotNull(dto.Products);
            Assert.Equal(id, dto.Id);
            Assert.Equal(1, dto.FC);
           
        }
    }
}
