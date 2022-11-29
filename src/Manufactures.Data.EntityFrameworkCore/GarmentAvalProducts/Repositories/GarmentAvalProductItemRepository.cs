using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentAvalProducts.Repositories
{
    public class GarmentAvalProductItemRepository : AggregateRepostory<GarmentAvalProductItem, GarmentAvalProductItemReadModel>, IGarmentAvalProductItemRepository
    {
        protected override GarmentAvalProductItem Map(GarmentAvalProductItemReadModel readModel)
        {
            return new GarmentAvalProductItem(readModel);
        }
    }
}