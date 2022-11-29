using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels
{
    public class GarmentSubconContractReadModel : ReadModelBase
    {
        public GarmentSubconContractReadModel(Guid identity) : base(identity)
        {
        }

        public string ContractType { get; internal set; }
        public string ContractNo { get; internal set; }
        public string AgreementNo { get; internal set; }
        public int SupplierId { get; internal set; }
        public string SupplierCode { get; internal set; }
        public string SupplierName { get; internal set; }
        public string JobType { get; internal set; }
        public string BPJNo { get; internal set; }
        public string FinishedGoodType { get; internal set; }
        public double Quantity { get; internal set; }
        public DateTimeOffset DueDate { get; internal set; }
        public DateTimeOffset ContractDate { get; internal set; }
        public bool IsUsed { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }

        public string SubconCategory { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string SKEPNo { get; internal set; }
        public DateTimeOffset AgreementDate { get; internal set; }
        public int CIF { get; internal set; }

        public virtual List<GarmentSubconContractItemReadModel> GarmentSubconContractItem { get; internal set; }
    }
}
