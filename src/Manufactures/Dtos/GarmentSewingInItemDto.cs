using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentSewingInItemDto : BaseDto
    {
        public GarmentSewingInItemDto(GarmentSewingInItem garmentSewingInItem)
        {
            Id = garmentSewingInItem.Identity;
            SewingInId = garmentSewingInItem.SewingInId;
            SewingOutItemId = garmentSewingInItem.SewingOutItemId;
            SewingOutDetailId = garmentSewingInItem.SewingOutDetailId;
            LoadingItemId = garmentSewingInItem.LoadingItemId;
            Product = new Product(garmentSewingInItem.ProductId.Value, garmentSewingInItem.ProductCode, garmentSewingInItem.ProductName);
            DesignColor = garmentSewingInItem.DesignColor;
            Size = new SizeValueObject(garmentSewingInItem.SizeId.Value, garmentSewingInItem.SizeName);
            Quantity = garmentSewingInItem.Quantity;
            Uom = new Uom(garmentSewingInItem.UomId.Value, garmentSewingInItem.UomUnit);
            Color = garmentSewingInItem.Color;
            RemainingQuantity = garmentSewingInItem.RemainingQuantity;
            BasicPrice = garmentSewingInItem.BasicPrice;
            Price = garmentSewingInItem.Price;
        }

        public GarmentSewingInItemDto(GarmentSewingInItemReadModel garmentSewingInItemReadModel)
        {
            Id = garmentSewingInItemReadModel.Identity;
            SewingInId = garmentSewingInItemReadModel.SewingInId;
            SewingOutItemId = garmentSewingInItemReadModel.SewingOutItemId;
            SewingOutDetailId = garmentSewingInItemReadModel.SewingOutDetailId;
            LoadingItemId = garmentSewingInItemReadModel.LoadingItemId;
            Product = new Product(garmentSewingInItemReadModel.ProductId, garmentSewingInItemReadModel.ProductCode, garmentSewingInItemReadModel.ProductName);
            DesignColor = garmentSewingInItemReadModel.DesignColor;
            Size = new SizeValueObject(garmentSewingInItemReadModel.SizeId, garmentSewingInItemReadModel.SizeName);
            Quantity = garmentSewingInItemReadModel.Quantity;
            Uom = new Uom(garmentSewingInItemReadModel.UomId, garmentSewingInItemReadModel.UomUnit);
            Color = garmentSewingInItemReadModel.Color;
            RemainingQuantity = garmentSewingInItemReadModel.RemainingQuantity;
            BasicPrice = garmentSewingInItemReadModel.BasicPrice;
            Price = garmentSewingInItemReadModel.Price;
        }


        public Guid Id { get; set; }
        public Guid SewingInId { get; set; }
        public Guid SewingOutItemId { get; set; }
        public Guid SewingOutDetailId { get; set; }
        public Guid LoadingItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }

    }
}