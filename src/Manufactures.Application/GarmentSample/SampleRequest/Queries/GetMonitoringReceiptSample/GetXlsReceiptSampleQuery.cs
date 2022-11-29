using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleRequest.Queries.GetMonitoringReceiptSample
{
    public class GetXlsReceiptSampleQuery : IQuery<MemoryStream>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string type { get; private set; }
        public string token { get; private set; }

        public DateTime receivedDateFrom { get; private set; }
        public DateTime receivedDateTo { get; private set; }

        public GetXlsReceiptSampleQuery(int page, int size, string order, DateTime receivedDateFrom, DateTime receivedDateTo, string token)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.receivedDateFrom = receivedDateFrom;
            this.receivedDateTo = receivedDateTo;
            this.token = token;
        }
    }
}