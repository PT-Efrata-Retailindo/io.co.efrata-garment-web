using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentExpenditureGoodDto : BaseDto
    {
        public GarmentExpenditureGoodDto(GarmentExpenditureGood garmentExpenditureGood)
        {
            Id = garmentExpenditureGood.Identity;
            ExpenditureGoodNo = garmentExpenditureGood.ExpenditureGoodNo;
            RONo = garmentExpenditureGood.RONo;
            Article = garmentExpenditureGood.Article;
            Unit = new UnitDepartment(garmentExpenditureGood.UnitId.Value, garmentExpenditureGood.UnitCode, garmentExpenditureGood.UnitName);
            ExpenditureDate = garmentExpenditureGood.ExpenditureDate;
            ExpenditureType = garmentExpenditureGood.ExpenditureType;
            Comodity = new GarmentComodity(garmentExpenditureGood.ComodityId.Value, garmentExpenditureGood.ComodityCode, garmentExpenditureGood.ComodityName);
            Buyer = new Buyer(garmentExpenditureGood.BuyerId.Value, garmentExpenditureGood.BuyerCode, garmentExpenditureGood.BuyerName);
            Invoice = garmentExpenditureGood.Invoice;
            ContractNo = garmentExpenditureGood.ContractNo;
            Carton = garmentExpenditureGood.Carton;
            Description = garmentExpenditureGood.Description;
            IsReceived = garmentExpenditureGood.IsReceived;
            PackingListId = garmentExpenditureGood.PackingListId;
            Items = new List<GarmentExpenditureGoodItemDto>();
        }
        public Guid Id { get; internal set; }
        public string ExpenditureGoodNo { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string ExpenditureType { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public Buyer Buyer { get; internal set; }
        public DateTimeOffset ExpenditureDate { get; internal set; }
        public string Invoice { get; internal set; }
        public int PackingListId { get; internal set; }
        public string ContractNo { get; internal set; }
        public double Carton { get; internal set; }
        public string Description { get; internal set; }
        public bool IsReceived { get; private set; }
        public virtual List<GarmentExpenditureGoodItemDto> Items { get; internal set; }
    }
}
