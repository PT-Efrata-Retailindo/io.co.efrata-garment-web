using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleExpenditureGoods
{
    public class GarmentSampleExpenditureGoodDto : BaseDto
    {
        public GarmentSampleExpenditureGoodDto(GarmentSampleExpenditureGood garmentSampleExpenditureGood)
        {
            Id = garmentSampleExpenditureGood.Identity;
            ExpenditureGoodNo = garmentSampleExpenditureGood.ExpenditureGoodNo;
            RONo = garmentSampleExpenditureGood.RONo;
            Article = garmentSampleExpenditureGood.Article;
            Unit = new UnitDepartment(garmentSampleExpenditureGood.UnitId.Value, garmentSampleExpenditureGood.UnitCode, garmentSampleExpenditureGood.UnitName);
            ExpenditureDate = garmentSampleExpenditureGood.ExpenditureDate;
            ExpenditureType = garmentSampleExpenditureGood.ExpenditureType;
            Comodity = new GarmentComodity(garmentSampleExpenditureGood.ComodityId.Value, garmentSampleExpenditureGood.ComodityCode, garmentSampleExpenditureGood.ComodityName);
            Buyer = new Buyer(garmentSampleExpenditureGood.BuyerId.Value, garmentSampleExpenditureGood.BuyerCode, garmentSampleExpenditureGood.BuyerName);
            Invoice = garmentSampleExpenditureGood.Invoice;
            ContractNo = garmentSampleExpenditureGood.ContractNo;
            Carton = garmentSampleExpenditureGood.Carton;
            Description = garmentSampleExpenditureGood.Description;
            IsReceived = garmentSampleExpenditureGood.IsReceived;
            PackingListId = garmentSampleExpenditureGood.PackingListId;
            Items = new List<GarmentSampleExpenditureGoodItemDto>();
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
        public virtual List<GarmentSampleExpenditureGoodItemDto> Items { get; internal set; }
    }
}
