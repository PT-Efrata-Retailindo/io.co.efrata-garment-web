using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalProducts.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class GarmentPreparingItemId : SingleValueObject<string>
    {
        public GarmentPreparingItemId(string value) : base(value)
        {
        }
    }
}