using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconShrinkagePanels
{
    public class GarmentServiceSubconShrinkagePanelDetailDto
    {
        public GarmentServiceSubconShrinkagePanelDetailDto(GarmentServiceSubconShrinkagePanelDetail garmentServiceSubconShrinkagePanelDetail)
        {
            Id = garmentServiceSubconShrinkagePanelDetail.Identity;
            ServiceSubconShrinkagePanelItemId = garmentServiceSubconShrinkagePanelDetail.ServiceSubconShrinkagePanelItemId;
            Product = new Product(garmentServiceSubconShrinkagePanelDetail.ProductId.Value, garmentServiceSubconShrinkagePanelDetail.ProductCode, garmentServiceSubconShrinkagePanelDetail.ProductName, garmentServiceSubconShrinkagePanelDetail.ProductRemark);
            DesignColor = garmentServiceSubconShrinkagePanelDetail.DesignColor;
            Quantity = garmentServiceSubconShrinkagePanelDetail.Quantity;
            Uom = new Uom(garmentServiceSubconShrinkagePanelDetail.UomId.Value, garmentServiceSubconShrinkagePanelDetail.UomUnit);
        }

        public Guid Id { get; set; }
        public Guid ServiceSubconShrinkagePanelItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public decimal Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Composition { get; set; }
    }
}
