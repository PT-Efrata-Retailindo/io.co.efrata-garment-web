using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class UomId : SingleValueObject<int>
    {
        public UomId(int value) : base(value)
        {
        }
    }
}
