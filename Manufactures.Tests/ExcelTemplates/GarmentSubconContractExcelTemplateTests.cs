using Manufactures.Application.GarmentSubcon.GarmentSubconContracts.ExcelTemplates;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.ExcelTemplates
{
    public class GarmentSubconContractExcelTemplateTests
    {
        [Fact]
        public void Generate_Return_Success()
        {
            // var department = new UnitDepartmentId(1);
            var id = Guid.NewGuid();
            var subconContract = new GarmentSubconContract(id, "", "", "", new SupplierId(1), "", "", "", "", "", 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "", "", new UomId(1), "", "", DateTimeOffset.Now, 1);

            GarmentSubconContractExcelDto dto = new GarmentSubconContractExcelDto(subconContract);
            
            var result = GarmentSubconContractAgreementExcelTemplate.GenerateExcelTemplate(dto);
            Assert.NotNull(result);
        }

        [Fact]
        public void Generate_Return_Success_Subcon_Garment()
        {
            // var department = new UnitDepartmentId(1);
            var id = Guid.NewGuid();
            var subconContract = new GarmentSubconContract(id, "SUBCON GARMENT", "", "", new SupplierId(1), "", "", "", "", "", 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "", "", new UomId(1), "", "", DateTimeOffset.Now, 0);

            GarmentSubconContractExcelDto dto = new GarmentSubconContractExcelDto(subconContract);
            var subconContractItem = new GarmentSubconContractItemExcelDto(new GarmentSubconContractItem(id, id, new Domain.Shared.ValueObjects.ProductId(1), "code", "FABRIC", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", 1));
            
            var items = new List<GarmentSubconContractItemExcelDto>()
            {
               subconContractItem
            };
            dto.GetType().GetProperty("Items").SetValue(dto, items);


            var result = GarmentSubconContractAgreementExcelTemplate.GenerateExcelTemplate(dto);
            Assert.NotNull(result);
        }

        [Fact]
        public void Generate_Return_Success_Subcon_Garment_Without_Items()
        {
            // var department = new UnitDepartmentId(1);
            var id = Guid.NewGuid();
            var subconContract = new GarmentSubconContract(id, "SUBCON GARMENT", "", "", new SupplierId(1), "", "", "", "", "", 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "", "", new UomId(1), "", "", DateTimeOffset.Now, 1);

            GarmentSubconContractExcelDto dto = new GarmentSubconContractExcelDto(subconContract);
            
            var result = GarmentSubconContractAgreementExcelTemplate.GenerateExcelTemplate(dto);
            Assert.NotNull(result);
        }

        [Fact]
        public void Generate_Return_Success_Subcon_Garment_Acc()
        {
            // var department = new UnitDepartmentId(1);
            var id = Guid.NewGuid();
            var subconContract = new GarmentSubconContract(id, "SUBCON GARMENT", "", "", new SupplierId(1), "", "", "", "", "", 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "", "", new UomId(1), "", "", DateTimeOffset.Now, 0);

            GarmentSubconContractExcelDto dto = new GarmentSubconContractExcelDto(subconContract);
            var subconContractItem = new GarmentSubconContractItemExcelDto(new GarmentSubconContractItem(id, id, new Domain.Shared.ValueObjects.ProductId(1), "code", "BUTTON", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", 1));

            var items = new List<GarmentSubconContractItemExcelDto>()
            {
               subconContractItem
            };
            dto.GetType().GetProperty("Items").SetValue(dto, items);


            var result = GarmentSubconContractAgreementExcelTemplate.GenerateExcelTemplate(dto);
            Assert.NotNull(result);
        }
    }
}
