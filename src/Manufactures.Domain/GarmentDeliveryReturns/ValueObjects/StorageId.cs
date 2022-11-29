using Moonlay.Domain;
using Newtonsoft.Json;


namespace Manufactures.Domain.GarmentDeliveryReturns.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class StorageId : SingleValueObject<int>
    {
        public StorageId(int value) : base(value)
        {
        }
    }
}