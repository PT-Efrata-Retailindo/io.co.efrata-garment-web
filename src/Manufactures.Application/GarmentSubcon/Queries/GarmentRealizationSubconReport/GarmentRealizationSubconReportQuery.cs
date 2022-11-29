using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentRealizationSubconReport
{
    public class GarmentRealizationSubconReportQuery : IQuery<GarmentRealizationSubconReportListViewModel>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string token { get; private set; }
        public string subconcontractNo { get; private set; }

        public GarmentRealizationSubconReportQuery(int page, int size, string order, string subconcontractNo, string token)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.subconcontractNo = subconcontractNo;
            this.token = token;
        }
    }
}
