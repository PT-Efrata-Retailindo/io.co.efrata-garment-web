using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Infrastructure.Domain.Queries;

namespace Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.SampahSapuan
{
    public class GetXlsSapuan_Out_Query : IQuery<MemoryStream>
    {
        public string token { get; private set; }
        public DateTime dateFrom { get; private set; }
        public DateTime dateTo { get; private set; }

        public GetXlsSapuan_Out_Query(DateTime dateFrom, DateTime dateTo, string token)
        {
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
            this.token = token;
        }
    }
}
