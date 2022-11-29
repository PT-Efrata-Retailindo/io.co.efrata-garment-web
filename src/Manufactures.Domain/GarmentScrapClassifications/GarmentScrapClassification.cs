using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentScrapClassifications.ReadModels;
using System;

namespace Manufactures.Domain.GarmentScrapClassifications
{
	public class GarmentScrapClassification : AggregateRoot<GarmentScrapClassification, GarmentScrapClassificationReadModel>
	{
		public string Code { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		

		public GarmentScrapClassification(Guid identity, string code, string name, string description) : base(identity)
		{
			
			Identity = identity;
			Code = code;
			Name = name;
			Description = description;
			ReadModel = new GarmentScrapClassificationReadModel(Identity)
			{
				Code=code,
				Name=name,
				Description=description
			};
			ReadModel.AddDomainEvent(new OnGarmentScrapClassificationPlaced(Identity));
		}

		public GarmentScrapClassification(GarmentScrapClassificationReadModel readModel) : base(readModel)
		{
			Code = readModel.Code;
			Name = readModel.Name;
			Description = readModel.Description;
		}
		
		public void Modify()
		{
			MarkModified();
		}

		protected override GarmentScrapClassification GetEntity()
		{
			return this;
		}

		public void setCode(string code)
		{
			if (this.Code != code)
			{
				this.Code = code ;
				ReadModel.Code = code;
			}
		}

		public void setName(string name)
		{
			if (this.Name != name)
			{
				this.Name = name;
				ReadModel.Name = name;
			}
		}
		public void setDescription(string desc)
		{
			if (this.Description != desc )
			{
				this.Description = desc ;
				ReadModel.Description = desc ;
			}
		}
	}
}
