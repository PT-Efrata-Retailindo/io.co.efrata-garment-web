using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleExpenditureGoods.Queries
{
    public class GarmentMonitoringSampleExpenditureGoodDto
    {
        public GarmentMonitoringSampleExpenditureGoodDto()
        {
        }

        public string expenditureGoodNo { get; internal set; }
        public string expenditureGoodType { get; internal set; }
        public DateTimeOffset? expenditureDate { get; internal set; }
        public DateTime? pebDate { get; internal set; }
        public string roNo { get; internal set; }
        public string buyerArticle { get; internal set; }
        public string buyerCode { get; internal set; }
        public string colour { get; internal set; }
        public string name { get; internal set; }
        public string unitname { get; internal set; }
        public double qty { get; internal set; }
        public string invoice { get; internal set; }
        public decimal price { get; internal set; }
        public decimal nominal { get; internal set; }

        public GarmentMonitoringSampleExpenditureGoodDto(GarmentMonitoringSampleExpenditureGoodDto garmentMonitoring)
        {

            expenditureGoodNo = garmentMonitoring.expenditureGoodNo;
            expenditureGoodType = garmentMonitoring.expenditureGoodType;
            expenditureDate = garmentMonitoring.expenditureDate;
            pebDate = garmentMonitoring.pebDate;
            roNo = garmentMonitoring.roNo;
            buyerArticle = garmentMonitoring.buyerArticle;
            colour = garmentMonitoring.colour;
            name = garmentMonitoring.name;
            qty = garmentMonitoring.qty;
            unitname = garmentMonitoring.unitname;
            invoice = garmentMonitoring.invoice;
            nominal = garmentMonitoring.nominal;
        }
    }
}
