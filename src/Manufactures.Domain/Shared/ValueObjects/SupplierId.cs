using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class SupplierId : SingleValueObject<int>
    {
        public SupplierId(int value) : base(value)
        {
        }
    }
}
