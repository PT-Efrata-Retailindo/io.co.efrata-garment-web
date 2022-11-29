using Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries.GetReportExpenditureGoods
{
    public class GarmentReportExpenditureGoodListViewModel
    {
        public List<GarmentReportExpenditureGoodDto> garmentReports { get; set; }
        public int count { get; set; }
    }
}
