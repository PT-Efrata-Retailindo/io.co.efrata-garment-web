using Infrastructure.Domain;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingOuts
{
	public class GarmentMonitoringFinishingReport : AggregateRoot<GarmentMonitoringFinishingReport, GarmentMonitoringFinishingReportReadModel>
	{
		//Enhance Jason Aug 2021
		public GarmentMonitoringFinishingReport(Guid identity, string roJob, string article, double Sewingqty, double finishingQtyPcs, double remainQty, string uomUnit) : base(identity)
		{
			RoJob = roJob;
			Article = article;
			FinishingQtyPcs = finishingQtyPcs;
			SewingQtyPcs = Sewingqty;
			RemainQty = remainQty;
			UomUnit = uomUnit;
			Identity = identity;

            ReadModel = new GarmentMonitoringFinishingReportReadModel(Identity)
            {

                RoJob = RoJob,
                Article = Article,
                Stock = Stock,
                SewingQtyPcs = SewingQtyPcs,
                RemainQty = RemainQty,
            };
		}

		public string RoJob { get; set; }
		public string Article { get; set; }
		public double Stock { get; set; }
		public double SewingQtyPcs { get; set; }
		public double FinishingQtyPcs { get; set; }
		public double RemainQty { get; set; }
		public string UomUnit { get; set; }

		public GarmentMonitoringFinishingReport(GarmentMonitoringFinishingReportReadModel readModel) : base(readModel)
		{
			RoJob = readModel.RoJob;
			Article = readModel.Article;
			Stock = readModel.Stock;
			FinishingQtyPcs = readModel.FinishingQtyPcs;
			SewingQtyPcs = readModel.SewingQtyPcs;
			RemainQty = readModel.RemainQty;
			UomUnit = readModel.UomUnit;
		}
		protected override GarmentMonitoringFinishingReport GetEntity()
		{
			return this;
		}
	}
}
