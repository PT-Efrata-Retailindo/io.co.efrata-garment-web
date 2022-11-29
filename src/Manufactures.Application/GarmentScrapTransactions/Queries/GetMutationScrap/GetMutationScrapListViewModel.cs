using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap
{
    public class GetMutationScrapListViewModel
    {
        public List<GetMutationScrapDto> garmentMonitorings { get; set; }
        public int count { get; set; }
    }
}
