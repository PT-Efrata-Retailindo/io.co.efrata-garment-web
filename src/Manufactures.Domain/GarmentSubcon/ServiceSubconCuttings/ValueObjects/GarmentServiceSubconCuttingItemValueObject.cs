using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ValueObjects
{
    public class GarmentServiceSubconCuttingItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid ServiceSubconCuttingId { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public List<GarmentServiceSubconCuttingDetailValueObject> Details { get; set; }
        public GarmentServiceSubconCuttingItemValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
