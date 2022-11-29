using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentCuttingInDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            var id = Guid.NewGuid();

            GarmentCuttingInDto dto = new GarmentCuttingInDto(new GarmentCuttingIn(new GarmentCuttingInReadModel(id)));
            dto.LastModifiedBy = "LastModifiedBy";
            dto.CreatedBy = "CreatedBy";
            dto.CutInNo = "CutInNo";
            dto.CuttingType = "CuttingType";
            dto.CuttingFrom = "CuttingFrom";
            dto.RONo = "RONo";
            dto.FC = 1;
            dto.Article = "Article";
            dto.CuttingInDate = DateTimeOffset.Now;
            dto.Unit = new Domain.Shared.ValueObjects.UnitDepartment();

            Assert.Equal("LastModifiedBy", dto.LastModifiedBy);
            Assert.Equal("CutInNo", dto.CutInNo);
            Assert.Equal("CuttingType", dto.CuttingType);
            Assert.Equal("RONo", dto.RONo);
            Assert.Equal("Article", dto.Article);
            Assert.Equal("CuttingFrom", dto.CuttingFrom);
            Assert.Equal("Article", dto.Article);
            Assert.Equal("Article", dto.Article);
            Assert.Equal(1, dto.FC);
            Assert.True(dto.CuttingInDate > DateTimeOffset.MinValue);
            Assert.Equal(id, dto.Id);
            Assert.NotNull(dto.Unit);
            Assert.NotNull(dto.CreatedBy);
        }
    }
}
