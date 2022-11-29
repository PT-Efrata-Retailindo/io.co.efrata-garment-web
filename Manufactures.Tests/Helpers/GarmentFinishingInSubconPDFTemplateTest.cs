using Manufactures.Domain.GarmentFinishingIns;
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
    public class GarmentFinishingInSubconPDFTemplateTest
    {
        [Fact]
        public void Generate_Return_Success()
        {
            // var department = new UnitDepartmentId(1);
            var id = Guid.NewGuid();
            var garmentFinishingIn = new GarmentFinishingIn(Guid.NewGuid(), "finishingPrinting", "PEMBELIAN", new UnitDepartmentId(1), "unitToCode", "unitToCode", "roNo", "Article", new UnitDepartmentId(1), "unitCode", "unitName", DateTimeOffset.Now, new GarmentComodityId(1), "comodityCode", "comodityName", 0, null, null);

            GarmentFinishingInDto dto = new GarmentFinishingInDto(garmentFinishingIn)
            {
                CreatedBy = "CreatedBy",
            };

            var finishingInItem = new GarmentFinishingInItemDto(new GarmentFinishingInItem(id, id, id, id, id, new SizeId(1), "sizeName", new ProductId(1), "productCode", "productName", "designColor", 0, 0, new UomId(1), "uomUnit", "color", 1, 1));

            var items = new List<GarmentFinishingInItemDto>()
                {
                   finishingInItem
                };
            dto.GetType().GetProperty("Items").SetValue(dto, items);


            var result = GarmentFinishingInSubconPDFTemplate.Generate(dto, "Buyer");
            Assert.NotNull(result);
        }

    }
}
