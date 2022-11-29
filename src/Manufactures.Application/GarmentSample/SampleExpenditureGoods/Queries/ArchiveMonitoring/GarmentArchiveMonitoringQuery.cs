using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleExpenditureGoods.Queries.ArchiveMonitoring
{
    public class GarmentArchiveMonitoringQuery : IQuery<GarmentArchiveMonitoringViewModel>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string token { get; private set; }
        public string type { get; private set; }
        public string roNo { get; private set; }
        public string comodity { get; private set; }

        public GarmentArchiveMonitoringQuery(int page, int size, string order, string type, string roNo, string comodity, string token)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.type = type;
            this.roNo = roNo;
            this.comodity = comodity;
            this.token = token;
        }
    }
}
