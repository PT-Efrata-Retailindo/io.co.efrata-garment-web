using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoods.ReadModels
{
    public class GarmentExpenditureGoodReadModel : ReadModelBase
    {
        public GarmentExpenditureGoodReadModel(Guid identity) : base(identity)
        {
        }

        public string ExpenditureGoodNo{ get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string ExpenditureType { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public DateTimeOffset ExpenditureDate { get; internal set; }
        public string Invoice { get; internal set; }
        public int PackingListId { get; internal set; }
        public string ContractNo { get; internal set; }
        public double Carton { get; internal set; }
        public string Description { get; internal set; }
        public bool IsReceived { get; internal set; }
		public string UId { get; set; }
		public virtual List<GarmentExpenditureGoodItemReadModel> Items { get; internal set; }


    }
}
