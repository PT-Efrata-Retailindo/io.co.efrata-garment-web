using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources.ReadModels
{
	public class GarmentScrapSourceReadModel : ReadModelBase
	{
		public GarmentScrapSourceReadModel(Guid identity) : base(identity)
		{

		}
		public string Code { get; internal set; }
		public string Name { get; internal set; }
		public string Description { get; internal set; }
		public string UId { get; internal set; }
	}
}
