using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentSubconContracts.Repositories
{
    public class GarmentSubconContractItemRepository : AggregateRepostory<GarmentSubconContractItem, GarmentSubconContractItemReadModel>, IGarmentSubconContractItemRepository
    {
        protected override GarmentSubconContractItem Map(GarmentSubconContractItemReadModel readModel)
        {
            return new GarmentSubconContractItem(readModel);
        }
    }
}
