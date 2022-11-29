using Moonlay.Domain;
using Newtonsoft.Json;

namespace Manufactures.Domain.GarmentDeliveryReturns.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class ProductId : SingleValueObject<int>
    {
        public ProductId(int value) : base(value)
        {
        }
    }
}
