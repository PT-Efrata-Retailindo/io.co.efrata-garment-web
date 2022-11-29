using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleAvalProducts.Repositories
{
    public class GarmentSampleAvalProductItemRepository : AggregateRepostory<GarmentSampleAvalProductItem, GarmentSampleAvalProductItemReadModel>, IGarmentSampleAvalProductItemRepository
    {
        protected override GarmentSampleAvalProductItem Map(GarmentSampleAvalProductItemReadModel readModel)
        {
            return new GarmentSampleAvalProductItem(readModel);
        }
    }
}
