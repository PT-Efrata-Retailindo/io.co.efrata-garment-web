using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels
{
    public class GarmentSubconCustomsInReadModel : ReadModelBase
    {
        public GarmentSubconCustomsInReadModel(Guid identity) : base(identity)
        {
        }
        public string BcNo { get; internal set; }
        public DateTimeOffset BcDate { get; internal set; }
        public string BcType { get; internal set; }
        public string SubconType { get; internal set; }
        public Guid SubconContractId { get; internal set; }
        public string SubconContractNo { get; internal set; }
        public int SupplierId { get; internal set; }
        public string SupplierCode { get; internal set; }
        public string SupplierName { get; internal set; }
        public string Remark { get; internal set; }

        public bool IsUsed { get; internal set; }
        public string SubconCategory { get; internal set; }
        public virtual List<GarmentSubconCustomsInItemReadModel> GarmentSubconCustomsInItem { get; internal set; }
    }
}
