using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.SubconDeliveryLetterOuts.Repositories
{
    public class GarmentSubconDeliveryLetterOutItemRepository : AggregateRepostory<GarmentSubconDeliveryLetterOutItem, GarmentSubconDeliveryLetterOutItemReadModel>, IGarmentSubconDeliveryLetterOutItemRepository
    {
        protected override GarmentSubconDeliveryLetterOutItem Map(GarmentSubconDeliveryLetterOutItemReadModel readModel)
        {
            return new GarmentSubconDeliveryLetterOutItem(readModel);
        }
    }
}