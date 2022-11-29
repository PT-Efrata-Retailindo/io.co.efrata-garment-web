using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ValueObjects
{
    public class GarmentServiceSubconSewingDetailValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid ServiceSubconSewingId { get; set; }
        public Guid SewingInId { get; set; }
        public Guid SewingInItemId { get; set; }
        public Product Product { get; set; }
        public UnitDepartment Unit { get; set; }
        public string DesignColor { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public bool IsSave { get; set; }
        public double SewingInQuantity { get; set; }
        public double TotalQuantity { get; set; }
        public string Remark { get; set; }
        public string Color { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
