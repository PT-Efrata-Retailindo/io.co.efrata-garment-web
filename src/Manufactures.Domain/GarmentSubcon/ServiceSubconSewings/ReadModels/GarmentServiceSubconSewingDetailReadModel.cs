using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels
{
    public class GarmentServiceSubconSewingDetailReadModel : ReadModelBase
    {
        public GarmentServiceSubconSewingDetailReadModel(Guid identity) : base(identity)
        {
        }

        public Guid ServiceSubconSewingItemId { get; internal set; }
        public Guid SewingInId { get; internal set; }
        public Guid SewingInItemId { get; internal set; }

        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public double Quantity { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string Remark { get; internal set; }
        public string Color { get; internal set; }

        public virtual GarmentServiceSubconSewingItemReadModel GarmentServiceSubconSewingItemIdentity { get; internal set; }
    }
}
