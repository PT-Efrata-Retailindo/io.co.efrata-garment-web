using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Dtos;
using Manufactures.Dtos.GarmentSubcon;
using Manufactures.Helpers.PDFTemplates.GarmentSubcon;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Helpers
{
    public class GarmentSubconDeliveryLetterOutPDFTemplateTest
    {
        [Fact]
        public void Generate_Return_Success()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSubconDeliveryLetterOutDto(new GarmentSubconDeliveryLetterOut(id, null, null, "", DateTimeOffset.Now, 1, "", "", 1, "", false, "", "SUBCON CUTTING SEWING", It.IsAny<int>(), "", It.IsAny<int>(), ""));

            var garmentSubconDLOutItem = new GarmentSubconDeliveryLetterOutItem(id, id, 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", "color", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType", new Guid(), "", "", "", It.IsAny<int>(), "");
            var items = new List<GarmentSubconDeliveryLetterOutItemDto>()
            {
                new GarmentSubconDeliveryLetterOutItemDto(garmentSubconDLOutItem)
            };
            dto.GetType().GetProperty("Items").SetValue(dto, items);

            var result = GarmentSubconDeliveryLetterOutPDFTemplate.Generate(dto, "Supplier");

            Assert.NotNull(result);
        }

        [Fact]
        public void Generate_Return_SUBCON_JASA_KOMPONEN_Success()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSubconDeliveryLetterOutDto(new GarmentSubconDeliveryLetterOut(id, null, null, "", DateTimeOffset.Now, 1, "", "", 1, "", false, "", "SUBCON JASA KOMPONEN", It.IsAny<int>(), "", It.IsAny<int>(), ""));

            var garmentSubconDLOutItem = new GarmentSubconDeliveryLetterOutItem(id, id, 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", "color", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType", new Guid(), "", "", "22PL0012", It.IsAny<int>(), "");
            var items = new List<GarmentSubconDeliveryLetterOutItemDto>()
            {
                new GarmentSubconDeliveryLetterOutItemDto(garmentSubconDLOutItem)
            };
            dto.GetType().GetProperty("Items").SetValue(dto, items);

            var result = GarmentSubconDeliveryLetterOutPDFTemplate.Generate(dto, "Supplier");

            Assert.NotNull(result);
        }

        [Fact]
        public void Generate_Return_SUBCON_JASA_GARMENT_WASH_Success()
        {
            Guid id = Guid.NewGuid();
            Guid IdSewing = Guid.NewGuid();
            var dto = new GarmentSubconDeliveryLetterOutDto(new GarmentSubconDeliveryLetterOut(id, null, null, "", DateTimeOffset.Now, 1, "", "", 1, "", false, "", "SUBCON JASA GARMENT WASH", It.IsAny<int>(), "", It.IsAny<int>(), ""));

            var garmentSubconDLOutItem = new GarmentSubconDeliveryLetterOutItem(id, id, 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", "color", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType", new Guid(), "", "", "", It.IsAny<int>(), "");
            GarmentSubconDeliveryLetterOutItemDto itemDto = new GarmentSubconDeliveryLetterOutItemDto(garmentSubconDLOutItem);
            var items = new List<GarmentSubconDeliveryLetterOutItemDto>()
            {
                itemDto
            };
            dto.GetType().GetProperty("Items").SetValue(dto, items);




            var subconSewingDto = new GarmentServiceSubconSewingDto(new GarmentServiceSubconSewing(IdSewing, "", DateTimeOffset.Now, true, new Domain.Shared.ValueObjects.BuyerId(1), "code", "name", 1, ""));
            var subconSewingDtoItem = new GarmentServiceSubconSewingItem(IdSewing, IdSewing, "", "", new Domain.Shared.ValueObjects.GarmentComodityId(1), "code", "name", new Domain.Shared.ValueObjects.BuyerId(1), "code", "name", new Domain.Shared.ValueObjects.UnitDepartmentId(1), "code", "name");
            var subconSewingDtoItems = new List<GarmentServiceSubconSewingItemDto>()
            {
                new GarmentServiceSubconSewingItemDto(subconSewingDtoItem)
            };
            subconSewingDto.GetType().GetProperty("Items").SetValue(subconSewingDto, subconSewingDtoItems);

            itemDto.SubconSewing = subconSewingDto;

            var result = GarmentSubconDeliveryLetterOutPDFTemplate.Generate(dto, "Supplier");

            Assert.NotNull(result);
        }

        [Fact]
        public void Generate_Return_SUBCON_BB_FABRIC_WASH_PRINT_Success()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSubconDeliveryLetterOutDto(new GarmentSubconDeliveryLetterOut(id, null, null, "", DateTimeOffset.Now, 1, "", "", 1, "", false, "", "SUBCON BB FABRIC WASH/PRINT", It.IsAny<int>(), "", It.IsAny<int>(), ""));

            var garmentSubconDLOutItem = new GarmentSubconDeliveryLetterOutItem(id, id, 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", "color", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType", new Guid(), "", "", "", It.IsAny<int>(), "");
            var items = new List<GarmentSubconDeliveryLetterOutItemDto>()
            {
                new GarmentSubconDeliveryLetterOutItemDto(garmentSubconDLOutItem)
            };
            dto.GetType().GetProperty("Items").SetValue(dto, items);

            var result = GarmentSubconDeliveryLetterOutPDFTemplate.Generate(dto, "Supplier");

            Assert.NotNull(result);
        }

        [Fact]
        public void Generate_Return_SUBCON_SEWING_Success()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSubconDeliveryLetterOutDto(new GarmentSubconDeliveryLetterOut(id, null, null, "", DateTimeOffset.Now, 1, "", "", 1, "", false, "", "SUBCON SEWING", It.IsAny<int>(), "", It.IsAny<int>(), ""));
            
            var garmentSubconDLOutItem = new GarmentSubconDeliveryLetterOutItem(id, id, 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", "color", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType", new Guid(), "", "", "", It.IsAny<int>(), "");
            GarmentSubconDeliveryLetterOutItemDto itemDto = new GarmentSubconDeliveryLetterOutItemDto(garmentSubconDLOutItem);
            var subconCuttingDto= new GarmentSubconCuttingOutDto(new GarmentSubconCuttingOut(id, "cutOutNo", "cuttingOutType", new Domain.Shared.ValueObjects.UnitDepartmentId(1), "unitFromCode", "unitFromName", DateTimeOffset.Now, "roNo", "article", new Domain.Shared.ValueObjects.GarmentComodityId(1), "comodityCode", "comodityName", 1, 1, "", false));
            var subconCuttingItem = new GarmentSubconCuttingOutItem(id, id, id, id, new Domain.Shared.ValueObjects.ProductId(1), "", "", "", 10);
            var subconCuttingItems = new List<GarmentSubconCuttingOutItemDto>()
            {
                new GarmentSubconCuttingOutItemDto(subconCuttingItem)
            };
            subconCuttingDto.GetType().GetProperty("Items").SetValue(subconCuttingDto, subconCuttingItems);
            itemDto.SubconCutting = subconCuttingDto;


            var items = new List<GarmentSubconDeliveryLetterOutItemDto>()
            {
                itemDto
            };

            
            dto.GetType().GetProperty("Items").SetValue(dto, items);

            var result = GarmentSubconDeliveryLetterOutPDFTemplate.Generate(dto, "Supplier");

            Assert.NotNull(result);
        }

    }
}
