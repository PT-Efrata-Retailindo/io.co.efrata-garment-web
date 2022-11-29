using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources.ReadModels
{
	public class GarmentScrapTransactionItemReadModel : ReadModelBase
	{
		public GarmentScrapTransactionItemReadModel(Guid identity) : base(identity)
		{

		}

		public Guid ScrapTransactionId { get; internal set; }
		public Guid ScrapClassificationId { get; internal set; }
		public string ScrapClassificationName { get; internal set; }
		public double Quantity { get; internal set; }
		public int UomId { get; internal set; }
		public string UomUnit { get; internal set; }
		public string Description { get; internal set; }
		public virtual GarmentScrapTransactionReadModel GarmentScrapTransactionIdentity { get; internal set; }
	}
}
