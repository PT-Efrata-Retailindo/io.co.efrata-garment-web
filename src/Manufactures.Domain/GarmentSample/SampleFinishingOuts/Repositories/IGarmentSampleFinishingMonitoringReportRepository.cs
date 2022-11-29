using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories
{
    public interface IGarmentSampleFinishingMonitoringReportRepository : IAggregateRepository<GarmentMonitoringSampleFinishingReport, GarmentMonitoringSampleFinishingReportReadModel>
    {
        IQueryable<GarmentMonitoringSampleFinishingReportReadModel> Read(int page, int size, string order, string keyword, string filter);
        IQueryable<GarmentMonitoringSampleFinishingReportReadModel> Read(int unit, DateTime dateFrom, DateTime dateTo);
    }

}
