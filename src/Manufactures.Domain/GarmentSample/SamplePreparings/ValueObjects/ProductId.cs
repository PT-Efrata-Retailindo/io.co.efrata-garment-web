using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SamplePreparings.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class ProductId : SingleValueObject<int>
    {
        public ProductId(int value) : base(value)
        {
        }
    }
}
