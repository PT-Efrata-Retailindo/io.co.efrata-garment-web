using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries
{
	public class GarmentMonitoringExpenditureGoodDto
	{
		public GarmentMonitoringExpenditureGoodDto()
		{
		}

		public string expenditureGoodNo { get; internal set; }
		public string expenditureGoodType { get; internal set; }
		public DateTimeOffset  ? expenditureDate { get; internal set; }
		public string pebDate { get; internal set; }
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
        public string pebNo { get; internal set; }
		public string currencyCode { get; internal set; }
		public string country { get; internal set; }
		public string buyerName { get; internal set; }
		public string comodityCode { get; internal set; }
		public string comodityName { get; internal set; }
		public string uomUnit { get; internal set; }
		public GarmentMonitoringExpenditureGoodDto(GarmentMonitoringExpenditureGoodDto garmentMonitoring)
		{

			expenditureGoodNo = garmentMonitoring.expenditureGoodNo;
			expenditureGoodType = garmentMonitoring.expenditureGoodType;
			expenditureDate = garmentMonitoring.expenditureDate;
			pebDate = garmentMonitoring.pebDate;
			//roNo = garmentMonitoring.roNo;
			//buyerArticle = garmentMonitoring.buyerArticle;
			//colour = garmentMonitoring.colour;
			//name = garmentMonitoring.name;
			qty = garmentMonitoring.qty;
            //unitname = garmentMonitoring.unitname;
            invoice = garmentMonitoring.invoice;
			//nominal = garmentMonitoring.nominal;
			pebNo = garmentMonitoring.pebNo;
			currencyCode = garmentMonitoring.currencyCode;
			country = garmentMonitoring.country;
			buyerName = garmentMonitoring.buyerName;
			comodityCode = garmentMonitoring.comodityCode;
			comodityName = garmentMonitoring.comodityName;
			uomUnit = garmentMonitoring.uomUnit;
		}
	}
}
