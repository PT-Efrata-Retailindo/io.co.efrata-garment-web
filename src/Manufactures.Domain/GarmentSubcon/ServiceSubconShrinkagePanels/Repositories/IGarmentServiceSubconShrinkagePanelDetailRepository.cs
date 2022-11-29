using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories
{
    public interface IGarmentServiceSubconShrinkagePanelDetailRepository : IAggregateRepository<GarmentServiceSubconShrinkagePanelDetail, GarmentServiceSubconShrinkagePanelDetailReadModel>
    {
    }
}
