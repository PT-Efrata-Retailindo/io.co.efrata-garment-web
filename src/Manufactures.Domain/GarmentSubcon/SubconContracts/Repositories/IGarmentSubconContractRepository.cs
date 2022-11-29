using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories
{
    public interface IGarmentSubconContractRepository : IAggregateRepository<GarmentSubconContract, GarmentSubconContractReadModel>
    {
        IQueryable<GarmentSubconContractReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}