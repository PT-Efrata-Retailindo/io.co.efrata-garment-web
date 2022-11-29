using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources
{
	public class GarmentScrapStock : AggregateRoot<GarmentScrapStock, GarmentScrapStockReadModel>
	{

		public Guid ScrapDestinationId { get; private set; }
		public string ScrapDestinationName { get; private set; }
		public Guid ScrapClassificationId { get; private set; }
		public string ScrapClassificationName { get; private set; }
		public double Quantity { get; private set; }
		public int UomId { get; private set; }
		public string UomUnit { get; private set; }

		protected override GarmentScrapStock GetEntity()
		{
			return this;
		}
		public GarmentScrapStock(Guid identity, Guid scrapDestinationId, string scrapDestinationName,Guid scrapClassificationId, string scrapClassificationName, double quanity,int uomId,string uomUnit) : base(identity)
		{
			Identity = identity;
			ScrapClassificationId = scrapClassificationId;
			ScrapClassificationName = scrapClassificationName;
			ScrapDestinationId = scrapDestinationId;
			ScrapDestinationName = scrapDestinationName;
			Quantity = quanity;
			UomId = uomId;
			UomUnit = uomUnit;
			ReadModel = new GarmentScrapStockReadModel(Identity)
			{
				ScrapClassificationId = scrapClassificationId,
				ScrapClassificationName = scrapClassificationName,
				ScrapDestinationId = scrapDestinationId,
				ScrapDestinationName = scrapDestinationName,
				Quantity = quanity,
				UomId = uomId,
				UomUnit = uomUnit,
			};

			ReadModel.AddDomainEvent(new OnGarmentScrapStockPlaced(Identity));
		}
		public GarmentScrapStock(GarmentScrapStockReadModel readModel) : base(readModel)
		{
			ScrapClassificationId = readModel.ScrapClassificationId;
			ScrapClassificationName = readModel.ScrapClassificationName;
			ScrapDestinationId = readModel.ScrapDestinationId;
			ScrapDestinationName = readModel.ScrapDestinationName;
			Quantity = readModel.Quantity;
			UomId = readModel.UomId;
			UomUnit = readModel.UomUnit;
		}

		public void SetQuantity(double Quantity)
		{
			if (this.Quantity != Quantity)
			{
				this.Quantity = Quantity;
				ReadModel.Quantity = Quantity;
			}
		}
		
		public void Modify()
		{
			MarkModified();
		}
	}
}
