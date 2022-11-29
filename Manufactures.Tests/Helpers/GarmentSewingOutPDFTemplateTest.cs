using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos;
using Manufactures.Helpers.PDFTemplates;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Helpers
{
    public class GarmentSewingOutPDFTemplateTest
    {
        [Fact]
        public void Generate_Return_Success()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSewingOutDto(new GarmentSewingOut(id, "sewingOutNo", new BuyerId(1), "BuyerCode", "BuyerName", new UnitDepartmentId(1), "unitToCode", "unitToName", "SewingTo", DateTimeOffset.Now, "RoNo", "Article", new UnitDepartmentId(1), "unitCode", "unitName", new GarmentComodityId(1), "comodityCode", "ComodityName", false));

            var garmentSewingOutItem = new GarmentSewingOutItem(id, id, id, id, new ProductId(1), "productCode", "productName", "designColor", new SizeId(1), "sizeName", 1, new UomId(1), "uomUnit", "Color", 1, 1, 1);
            var items = new List<GarmentSewingOutItemDto>()
            {
                new GarmentSewingOutItemDto(garmentSewingOutItem)
            };
            dto.GetType().GetProperty("Items").SetValue(dto, items);

            var result = GarmentSewingOutPDFTemplate.Generate(dto, "Buyer");
            
            Assert.NotNull(result);
        }
    }
}
