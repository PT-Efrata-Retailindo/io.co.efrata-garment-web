using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels
{
    public class GarmentSubconDeliveryLetterOutItemReadModel : ReadModelBase
    {
        public GarmentSubconDeliveryLetterOutItemReadModel(Guid identity) : base(identity)
        {
        }
        public Guid SubconDeliveryLetterOutId { get; internal set; }
        public int UENItemId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string ProductRemark { get; internal set; }

        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }

        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public int UomOutId { get; internal set; }
        public string UomOutUnit { get; internal set; }

        public string FabricType { get; internal set; }
        public int QtyPacking { get; internal set; }
        public string UomSatuanUnit { get; internal set; }

        #region Cutting
        public string RONo { get; internal set; }
        public Guid SubconId { get; internal set; }
        public string POSerialNumber { get; internal set; }
        public string SubconNo { get; internal set; }

        #endregion
        public virtual GarmentSubconDeliveryLetterOutReadModel GarmentSubconDeliveryLetterOut { get; internal set; }
    }
}
