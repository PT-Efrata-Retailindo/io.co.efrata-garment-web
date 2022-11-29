using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos;
using Manufactures.Helpers.PDFTemplates;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Helpers
{
    public class GarmentFinishingOutPDFTemplateTest
    {
        [Fact]
        public void Generate_Return_Success()
        {
            // var department = new UnitDepartmentId(1);
            var id = Guid.NewGuid();
            var garmentFinishingOut = new GarmentFinishingOut(Guid.NewGuid(), "finishingPrinting", new UnitDepartmentId(1), "unitToCode", "unitToCode", "finishingTo", DateTimeOffset.Now, "roNo", "Article", new UnitDepartmentId(1), "unitCode", "unitName", new GarmentComodityId(1), "comodityCode", "comodityName", false);

            GarmentFinishingOutDto dto = new GarmentFinishingOutDto(garmentFinishingOut)
            {
                CreatedBy = "CreatedBy",

            };
            var finishingOutItem = new GarmentFinishingOutItemDto(new GarmentFinishingOutItem(id, id, id, id, new ProductId(1), "productCode", "productName", "designColor", new SizeId(1), "sizeName", 1, new UomId(1), "uomUnit", "color", 1, 1, 1));
            finishingOutItem.Details = new List<GarmentFinishingOutDetailDto>()
            {
                new GarmentFinishingOutDetailDto(new GarmentFinishingOutDetail(id,id,new SizeId(1),"sizeName",1,new UomId(1),"uomUnit"))
            };

            var items = new List<GarmentFinishingOutItemDto>()
            {
               finishingOutItem
            };
            dto.GetType().GetProperty("Items").SetValue(dto, items);


            var result = GarmentFinishingOutPDFTemplate.Generate(dto, "Buyer");
            Assert.NotNull(result);
        }

    }
}