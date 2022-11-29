using Manufactures.Domain.GarmentSubcon.SubconContracts;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconContracts.ExcelTemplates
{
    public class GarmentSubconContractItemExcelDto
    {
        public GarmentSubconContractItemExcelDto(GarmentSubconContractItem garmentSubconContractItem)
        {
            Id = garmentSubconContractItem.Identity;
            SubconContractId = garmentSubconContractItem.SubconContractId;
            Product = new Product(garmentSubconContractItem.ProductId.Value, garmentSubconContractItem.ProductCode, garmentSubconContractItem.ProductName);
            Quantity = garmentSubconContractItem.Quantity;
            Uom = new Uom(garmentSubconContractItem.UomId.Value, garmentSubconContractItem.UomUnit);

        }

        public Guid Id { get; set; }
        public Guid SubconContractId { get; set; }

        public Product Product { get; set; }
        public double Quantity { get; set; }

        public Uom Uom { get; set; }
    }
}
