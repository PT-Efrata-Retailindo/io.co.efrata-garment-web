using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconSewings.Repositories
{
    public class GarmentServiceSubconSewingDetailRepository : AggregateRepostory<GarmentServiceSubconSewingDetail, GarmentServiceSubconSewingDetailReadModel>, IGarmentServiceSubconSewingDetailRepository
    {
        protected override GarmentServiceSubconSewingDetail Map(GarmentServiceSubconSewingDetailReadModel readModel)
        {
            return new GarmentServiceSubconSewingDetail(readModel);
        }
    }
}
