using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods
{
    public class GarmentReportExpenditureGoodDto
    {
        public GarmentReportExpenditureGoodDto()
        {
        }

        public string ExpenditureGoodId { get; set; }
        public string RO { get; set; }
        public string Article { get; set; }
        public string UnitCode { get; set; }
        public string BuyerContract { get; set; }
        public string ExpenditureTypeName { get; set; }
        public string Description { get; set; }
        public string ComodityName { get; set; }
        public string ComodityCode { get; set; }
        public string SizeNumber { get; set; }
        public string Descriptionb { get; set; }
        public double Qty { get; set; }

        public GarmentReportExpenditureGoodDto(GarmentReportExpenditureGoodDto getReportExpenditure)
        {

            ExpenditureGoodId = getReportExpenditure.ExpenditureGoodId;
            RO = getReportExpenditure.RO;
            Article = getReportExpenditure.Article;
            UnitCode = getReportExpenditure.UnitCode;
            BuyerContract = getReportExpenditure.BuyerContract;
            ExpenditureTypeName = getReportExpenditure.ExpenditureTypeName;
            Description = getReportExpenditure.Description;
            ComodityName = getReportExpenditure.ComodityName;
            ComodityCode = getReportExpenditure.ComodityCode;
            SizeNumber = getReportExpenditure.SizeNumber;
            Descriptionb = getReportExpenditure.Descriptionb;
            Qty = getReportExpenditure.Qty;
        }

    }
}
