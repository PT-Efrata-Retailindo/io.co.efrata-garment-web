using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories
{
    public interface IGarmentSubconContractItemRepository : IAggregateRepository<GarmentSubconContractItem, GarmentSubconContractItemReadModel>
    {
    }
}
