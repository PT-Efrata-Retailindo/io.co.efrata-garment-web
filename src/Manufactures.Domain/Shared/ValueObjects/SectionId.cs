using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class SectionId : SingleValueObject<int>
    {
        public SectionId(int value) : base(value)
        {
        }
    }
}
