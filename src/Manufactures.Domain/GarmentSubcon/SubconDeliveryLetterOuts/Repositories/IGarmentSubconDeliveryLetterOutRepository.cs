using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories
{
    public interface IGarmentSubconDeliveryLetterOutRepository : IAggregateRepository<GarmentSubconDeliveryLetterOut, GarmentSubconDeliveryLetterOutReadModel>
    {
        IQueryable<GarmentSubconDeliveryLetterOutReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
