using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentSubconContractItemDto : BaseDto
    {
        public GarmentSubconContractItemDto(GarmentSubconContractItem garmentSubconContractItem)
        {
            Id = garmentSubconContractItem.Identity;
            SubconContractId = garmentSubconContractItem.SubconContractId;
            Product = new Product(garmentSubconContractItem.ProductId.Value, garmentSubconContractItem.ProductCode, garmentSubconContractItem.ProductName);
            Quantity = garmentSubconContractItem.Quantity;
            Uom = new Uom(garmentSubconContractItem.UomId.Value, garmentSubconContractItem.UomUnit);
            CIFItem = garmentSubconContractItem.CIFItem;

        }

        public Guid Id { get; set; }
        public Guid SubconContractId { get; set; }

        public Product Product { get; set; }
        public double Quantity { get; set; }

        public Uom Uom { get; set; }
        public int CIFItem { get; set; }
    }
}

