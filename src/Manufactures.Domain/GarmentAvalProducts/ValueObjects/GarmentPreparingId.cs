using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalProducts.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class GarmentPreparingId : SingleValueObject<string>
    {
        public GarmentPreparingId(string value) : base(value)
        {
        }
    }
}