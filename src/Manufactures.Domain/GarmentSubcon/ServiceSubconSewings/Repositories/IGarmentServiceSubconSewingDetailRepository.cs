using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories
{
    public interface IGarmentServiceSubconSewingDetailRepository : IAggregateRepository<GarmentServiceSubconSewingDetail, GarmentServiceSubconSewingDetailReadModel>
    {
    }
}
