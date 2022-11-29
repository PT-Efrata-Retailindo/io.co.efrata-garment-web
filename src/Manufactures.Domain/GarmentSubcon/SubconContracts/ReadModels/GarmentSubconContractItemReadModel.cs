using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels
{
    public class GarmentSubconContractItemReadModel : ReadModelBase
    {
        public GarmentSubconContractItemReadModel(Guid identity) : base(identity)
        {
        }
        public Guid SubconContractId { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public int CIFItem { get; internal set; }

        public virtual GarmentSubconContractReadModel GarmentSubconContract { get; internal set; }
    }
}
