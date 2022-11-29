using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class UnitSenderId : SingleValueObject<int>
    {
        public UnitSenderId(int value) : base(value)
        {
        }
    }
}
