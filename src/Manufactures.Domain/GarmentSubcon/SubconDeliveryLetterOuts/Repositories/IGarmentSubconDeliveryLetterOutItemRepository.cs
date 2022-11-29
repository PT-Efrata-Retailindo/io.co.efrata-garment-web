using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories
{
    public interface IGarmentSubconDeliveryLetterOutItemRepository : IAggregateRepository<GarmentSubconDeliveryLetterOutItem, GarmentSubconDeliveryLetterOutItemReadModel>
    {
    }
}
