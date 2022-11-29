using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Infrastructure.Domain.Queries;


namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconShrinkagePanels.ExcelTemplates
{
    public class GetXlsSubconServiceSubconShrinkagePanelsQuery : IQuery<MemoryStream>
    {
        public string token { get; private set; }
        public DateTime dateFrom { get; private set; }
        public DateTime dateTo { get; private set; }

        public GetXlsSubconServiceSubconShrinkagePanelsQuery(DateTime dateFrom, DateTime dateTo, string token)
        {
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
            this.token = token;
        }
    }
}
