using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources
{
	public class GarmentScrapTransactionItem : AggregateRoot<GarmentScrapTransactionItem, GarmentScrapTransactionItemReadModel>
	{
		public Guid ScrapTransactionId { get; private set; }
		public Guid ScrapClassificationId { get; private set; }
		public string ScrapClassificationName { get; private set; }
		public double Quantity { get; private set; }
		public int UomId { get; private set; }
		public string UomUnit { get; private set; }
		public string Description { get; private set; }

		public GarmentScrapTransactionItem(Guid identity, Guid scrapTransactionId, Guid scrapClassificationId, string scrapClassificationName, double quantity,int uomId, string uomUnit, string description) : base(identity)
		{
			//MarkTransient();

			Identity = identity;
			ScrapTransactionId = scrapTransactionId;
			ScrapClassificationId = scrapClassificationId;
			ScrapClassificationName = scrapClassificationName;
			Quantity = quantity;
			UomId = uomId;
			UomUnit = uomUnit;
			Description = description;

			ReadModel = new GarmentScrapTransactionItemReadModel(identity)
			{
				ScrapTransactionId = ScrapTransactionId,
				ScrapClassificationId = ScrapClassificationId,
				ScrapClassificationName = ScrapClassificationName,
				Quantity = Quantity,
				UomId = UomId,
				UomUnit = UomUnit,
				Description = Description,
			};

			ReadModel.AddDomainEvent(new OnGarmentScrapTransactionItemPlaced(Identity));
		}

		public GarmentScrapTransactionItem(GarmentScrapTransactionItemReadModel readModel) : base(readModel)
		{
			ScrapTransactionId = readModel.ScrapTransactionId;
			ScrapClassificationId = readModel.ScrapClassificationId;
			ScrapClassificationName = readModel.ScrapClassificationName;
			Quantity = readModel.Quantity;
			UomId = readModel.UomId;
			UomUnit = readModel.UomUnit;
			Description = readModel.Description;
		}

		protected override GarmentScrapTransactionItem GetEntity()
		{
			return this;
		}
		public void SetQuantity(double Quantity)
		{
			if (this.Quantity != Quantity)
			{
				this.Quantity = Quantity;
				ReadModel.Quantity = Quantity;
			}
		}
		public void SetDescription(string Description)
		{
			if (this.Description != Description)
			{
				this.Description = Description;
				ReadModel.Description = Description;
			}
		}
		public void Modify()
		{
			MarkModified();
		}

	}
}
