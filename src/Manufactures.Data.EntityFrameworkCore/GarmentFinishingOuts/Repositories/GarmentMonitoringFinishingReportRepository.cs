using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingOuts.Repositories
{
    public class GarmentMonitoringFinishingReportRepository : AggregateRepostory<GarmentMonitoringFinishingReport, GarmentMonitoringFinishingReportReadModel>, IGarmentMonitoringFinishingReportRepository
	{
		public IQueryable<GarmentMonitoringFinishingReportReadModel> Read(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;//.Where(d => d.CuttingOutType != "SUBKON");

			return data;
		}

		protected override GarmentMonitoringFinishingReport Map(GarmentMonitoringFinishingReportReadModel readModel)
		{
			return new GarmentMonitoringFinishingReport(readModel);
		}

        public IQueryable<GarmentMonitoringFinishingReportReadModel> Read(int unitId, DateTime dateFrom, DateTime dateTo)
        {
			//Enhance Jason Aug 2021 : GarmentMonitoringFinishingReport as Table Valued Function
			var reportData =  Query.FromSql("select * from [dbo].[GarmentMonitoringFinishingReport](@p0,@p1,@p2)", unitId, dateFrom, dateTo);
			return reportData;
		}
    }
}
