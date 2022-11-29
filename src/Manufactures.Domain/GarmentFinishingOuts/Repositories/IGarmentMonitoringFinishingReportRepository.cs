using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingOuts.Repositories
{
	public interface IGarmentMonitoringFinishingReportRepository : IAggregateRepository<GarmentMonitoringFinishingReport, GarmentMonitoringFinishingReportReadModel>
	{
		//Enhance Jason Aug 2021
		IQueryable<GarmentMonitoringFinishingReportReadModel> Read(int page, int size, string order, string keyword, string filter);
		IQueryable<GarmentMonitoringFinishingReportReadModel> Read(int unit, DateTime dateFrom, DateTime dateTo);
    }
}
