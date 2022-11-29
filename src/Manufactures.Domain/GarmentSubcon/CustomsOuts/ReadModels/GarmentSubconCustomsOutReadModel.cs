using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels
{
    public class GarmentSubconCustomsOutReadModel : ReadModelBase
    {
        public GarmentSubconCustomsOutReadModel(Guid identity) : base(identity)
        {
        }
        public string CustomsOutNo { get; internal set; }
        public DateTimeOffset CustomsOutDate { get; internal set; }
        public string CustomsOutType { get; internal set; }
        public string SubconType { get; internal set; }
        public Guid SubconContractId { get; internal set; }
        public string SubconContractNo { get; internal set; }
        public int SupplierId { get; internal set; }
        public string SupplierCode { get; internal set; }
        public string SupplierName { get; internal set; }
        public string Remark { get; internal set; }
        public string SubconCategory { get; internal set; }
        public virtual List<GarmentSubconCustomsOutItemReadModel> GarmentSubconCustomsOutItem { get; internal set; }
    }
}
