using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishingOuts.Repositories
{
    public class GarmentMonitoringSampleFinishingReportRepository : AggregateRepostory<GarmentMonitoringSampleFinishingReport, GarmentMonitoringSampleFinishingReportReadModel>, IGarmentSampleFinishingMonitoringReportRepository
    {
        public IQueryable<GarmentMonitoringSampleFinishingReportReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;//.Where(d => d.CuttingOutType != "SUBKON");

            return data;
        }

        protected override GarmentMonitoringSampleFinishingReport Map(GarmentMonitoringSampleFinishingReportReadModel readModel)
        {
            return new GarmentMonitoringSampleFinishingReport(readModel);
        }

        public IQueryable<GarmentMonitoringSampleFinishingReportReadModel> Read(int unitId, DateTime dateFrom, DateTime dateTo)
        {
            //Enhance Jason Aug 2021 : GarmentMonitoringFinishingReport as Table Valued Function
            var reportData = Query.FromSql("select * from [dbo].[GarmentMonitoringSampleFinishingReport](@p0,@p1,@p2)", unitId, dateFrom, dateTo);
            return reportData;
        }
    }
}
