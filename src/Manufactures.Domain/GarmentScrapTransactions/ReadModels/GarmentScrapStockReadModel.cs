using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources.ReadModels
{
	public class GarmentScrapStockReadModel : ReadModelBase
	{
		public GarmentScrapStockReadModel(Guid identity) : base(identity)
		{

		}

		public Guid ScrapDestinationId { get; internal set; }
		public string ScrapDestinationName { get; internal set; }
		public Guid ScrapClassificationId { get; internal set; }
		public string ScrapClassificationName { get; internal set; }
		public double Quantity { get; internal set; }
		public int UomId { get; internal set; }
		public string UomUnit { get; internal set; }
	}
}
