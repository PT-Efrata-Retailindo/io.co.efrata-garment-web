using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconSewingDetailDto
    {
        public GarmentServiceSubconSewingDetailDto(GarmentServiceSubconSewingDetail garmentServiceSubconSewingDetail)
        {
            Id = garmentServiceSubconSewingDetail.Identity;
            ServiceSubconSewingItemId = garmentServiceSubconSewingDetail.ServiceSubconSewingItemId;
            SewingInId = garmentServiceSubconSewingDetail.SewingInId;
            SewingInItemId = garmentServiceSubconSewingDetail.SewingInItemId;
            Product = new Product(garmentServiceSubconSewingDetail.ProductId.Value, garmentServiceSubconSewingDetail.ProductCode, garmentServiceSubconSewingDetail.ProductName);
            DesignColor = garmentServiceSubconSewingDetail.DesignColor;
            Quantity = garmentServiceSubconSewingDetail.Quantity;
            Unit = new UnitDepartment(garmentServiceSubconSewingDetail.UnitId.Value, garmentServiceSubconSewingDetail.UnitCode, garmentServiceSubconSewingDetail.UnitName);
            Uom = new Uom(garmentServiceSubconSewingDetail.UomId.Value, garmentServiceSubconSewingDetail.UomUnit);
            Remark = garmentServiceSubconSewingDetail.Remark;
            Color = garmentServiceSubconSewingDetail.Color;
        }


        public Guid Id { get; set; }
        public Guid ServiceSubconSewingItemId { get; set; }
        public Guid SewingInId { get; set; }
        public Guid SewingInItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public UnitDepartment Unit { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Remark { get; set; }
        public string Color { get; set; }
    }
}
