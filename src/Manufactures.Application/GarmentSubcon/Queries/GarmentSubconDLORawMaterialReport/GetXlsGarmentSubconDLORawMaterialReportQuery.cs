using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLORawMaterialReport
{
    public class GetXlsGarmentSubconDLORawMaterialReportQuery : IQuery<MemoryStream>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public DateTime dateFrom { get; private set; }
        public DateTime dateTo { get; private set; }

        public GetXlsGarmentSubconDLORawMaterialReportQuery(int page, int size, string order, DateTime dateFrom, DateTime dateTo)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
        }
    }
}
