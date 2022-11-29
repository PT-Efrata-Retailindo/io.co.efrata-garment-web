using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSubconCuttings.Queries.GetAllGarmentSubconCuttings
{
    public class GarmentSubconCuttingDto
    {
        public GarmentSubconCuttingDto(GarmentSubconCutting s)
        {
            Id = s.Identity;
            RONo = s.RONo;
            Size = new SizeValueObject(s.SizeId.Value, s.SizeName);
            Quantity = s.Quantity;
            Product = new Product(s.ProductId.Value, s.ProductCode, s.ProductName);
            Comodity = new GarmentComodity(s.ComodityId.Value, s.ComodityCode, s.ComodityName);
            DesignColor = s.DesignColor;
            Remark = s.Remark;
            BasicPrice = s.BasicPrice;
            FinishingInQuantity = s.FinishingInQuantity;
        }

        public Guid Id { get; private set; }

        public string RONo { get; private set; }
        public SizeValueObject Size { get; private set; }
        public double Quantity { get; private set; }
        public Product Product { get; private set; }
        public GarmentComodity Comodity { get; private set; }
        public string DesignColor { get; private set; }
        public string Remark { get; private set; }
        public double BasicPrice { get; private set; }
        public double FinishingInQuantity { get; private set; }
    }
}
