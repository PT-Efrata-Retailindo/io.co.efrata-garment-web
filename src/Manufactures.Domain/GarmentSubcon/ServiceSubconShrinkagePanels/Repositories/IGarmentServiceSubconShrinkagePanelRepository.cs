using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories
{
    public interface IGarmentServiceSubconShrinkagePanelRepository : IAggregateRepository<GarmentServiceSubconShrinkagePanel, GarmentServiceSubconShrinkagePanelReadModel>
    {
        IQueryable<GarmentServiceSubconShrinkagePanelReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
