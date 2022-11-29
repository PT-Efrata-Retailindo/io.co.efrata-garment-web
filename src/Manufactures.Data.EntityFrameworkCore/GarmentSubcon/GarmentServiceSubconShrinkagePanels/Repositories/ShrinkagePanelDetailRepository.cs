using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconShrinkagePanels.Repositories
{
    public class ShrinkagePanelDetailRepository : AggregateRepostory<GarmentServiceSubconShrinkagePanelDetail, GarmentServiceSubconShrinkagePanelDetailReadModel>, IGarmentServiceSubconShrinkagePanelDetailRepository
    {
        protected override GarmentServiceSubconShrinkagePanelDetail Map(GarmentServiceSubconShrinkagePanelDetailReadModel readModel)
        {
            return new GarmentServiceSubconShrinkagePanelDetail(readModel);
        }
    }
}
