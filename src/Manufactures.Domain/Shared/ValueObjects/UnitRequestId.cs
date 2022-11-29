using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class UnitRequestId : SingleValueObject<int>
    {
        public UnitRequestId(int value) : base(value)
        {
        }
    }
}
