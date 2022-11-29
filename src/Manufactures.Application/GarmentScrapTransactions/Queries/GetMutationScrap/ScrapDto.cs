using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap
{
    public class ScrapDto
    {
        public ScrapDto()
        {
        }

        public string TransactionNo { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public string ScrapSourceName { get; set; }
        public double Quantity { get; set; }
        public string UomUnit { get; set; }

        public ScrapDto(ScrapDto scrapDto)
        {
            TransactionNo = scrapDto.TransactionNo;
            TransactionDate = scrapDto.TransactionDate;
            ScrapSourceName = scrapDto.ScrapSourceName;
            Quantity = scrapDto.Quantity;

        }

    }
}
