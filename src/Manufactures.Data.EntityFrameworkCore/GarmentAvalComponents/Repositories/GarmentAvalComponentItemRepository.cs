using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.GarmentAvalComponents.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.GarmentAvalComponents.Repositories
{
    public class GarmentAvalComponentItemRepository : AggregateRepostory<GarmentAvalComponentItem, GarmentAvalComponentItemReadModel>, IGarmentAvalComponentItemRepository
    {
        protected override GarmentAvalComponentItem Map(GarmentAvalComponentItemReadModel readModel)
        {
            return new GarmentAvalComponentItem(readModel);
        }
    }
}
