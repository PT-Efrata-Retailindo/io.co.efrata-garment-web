using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapClassifications.ReadModels
{
	public class GarmentScrapClassificationReadModel : ReadModelBase
	{
		public GarmentScrapClassificationReadModel(Guid identity) : base(identity)
		{

		}

		public string Code { get; internal set; }
		public string Name { get; internal set; }
		public string Description { get; internal set; }
		public string UId { get; internal set; }
	}
}
