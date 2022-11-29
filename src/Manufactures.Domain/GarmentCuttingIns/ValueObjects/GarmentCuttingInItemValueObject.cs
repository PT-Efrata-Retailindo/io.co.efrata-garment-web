using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingIns.ValueObjects
{
    public class GarmentCuttingInItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid PreparingId { get; set; }
        public int UENId { get; set; }
        public string UENNo { get; set; }
        public Guid SewingOutId { get; set; }
        public string SewingOutNo { get; set; }
        public List<GarmentCuttingInDetailValueObject> Details { get; set; }

        public GarmentCuttingInItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
