using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleExpenditureGoods.Queries.ArchiveMonitoring
{
    public class GarmentArchiveMonitoringDto
    {
        public GarmentArchiveMonitoringDto()
        {
        }

        public string archiveType { get; internal set; }
        public string roNo { get; internal set; }
        public string article { get; internal set; }
        public string buyer { get; internal set; }
        public string comodity { get; internal set; }
        public string size { get; internal set; }
        public double qty { get; internal set; }
        public string uom { get; internal set; }
        public string description { get; internal set; }

        public GarmentArchiveMonitoringDto(GarmentArchiveMonitoringDto garmentMonitoring)
        {

            archiveType = garmentMonitoring.archiveType;
            roNo = garmentMonitoring.roNo;
            article = garmentMonitoring.article;
            buyer = garmentMonitoring.buyer;
            comodity = garmentMonitoring.comodity;
            qty = garmentMonitoring.qty;
            size = garmentMonitoring.size;
            uom = garmentMonitoring.uom;
            description = garmentMonitoring.description;
        }
    }
}
