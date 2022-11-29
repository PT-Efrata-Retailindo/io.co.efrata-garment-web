using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentDeliveryReturns.ReadModels
{
    public class GarmentDeliveryReturnReadModel : ReadModelBase
    {
        public GarmentDeliveryReturnReadModel(Guid identity) : base(identity)
        {
            
        }

        public string DRNo { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int UnitDOId { get; internal set; }
        public string UnitDONo { get; internal set; }
        public int UENId { get; internal set; }
        public string PreparingId { get; internal set; }
        public DateTimeOffset? ReturnDate { get; internal set; }
        public string ReturnType { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public int StorageId { get; internal set; }
        public string StorageCode { get; internal set; }
        public string StorageName { get; internal set; }
        public bool IsUsed { get; internal set; }
		public string UId { get; set; }
        public virtual List<GarmentDeliveryReturnItemReadModel> GarmentDeliveryReturnItem { get; internal set; }
    }
}