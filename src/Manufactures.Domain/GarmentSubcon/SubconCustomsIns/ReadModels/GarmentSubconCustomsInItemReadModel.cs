using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels
{
    public class GarmentSubconCustomsInItemReadModel : ReadModelBase
    {
        public GarmentSubconCustomsInItemReadModel(Guid identity) : base(identity)
        {
        }
        public int SupplierId { get; internal set; }
        public string SupplierCode { get; internal set; }
        public string SupplierName { get; internal set; }
        public int DoId { get; internal set; }
        public string DoNo { get; internal set; }
        public decimal Quantity { get; internal set; }

        public Guid SubconCustomsInId { get; internal set; }

        public virtual GarmentSubconCustomsInReadModel GarmentSubconCustomsIn { get; internal set; }
    }
}
