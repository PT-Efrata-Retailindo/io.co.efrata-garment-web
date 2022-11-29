using Moonlay.Domain;
using Newtonsoft.Json;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class GarmentComodityId : SingleValueObject<int>
    {
        public GarmentComodityId(int value) : base(value)
        {
        }
    }
}