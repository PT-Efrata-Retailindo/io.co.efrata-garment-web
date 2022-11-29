using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources
{
	public class GarmentScrapSource : AggregateRoot<GarmentScrapSource, GarmentScrapSourceReadModel>
	{

		public string Code { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }

		protected override GarmentScrapSource GetEntity()
		{
			return this;
		}
		public GarmentScrapSource(Guid identity, string code, string name, string description) : base(identity)
		{
			Identity = identity;
			Code = code;
			Name = name;
			Description = description;
			ReadModel = new GarmentScrapSourceReadModel(Identity)
			{
				Code = code,
				Name = name,
				Description = description,
			};

			ReadModel.AddDomainEvent(new OnGarmentScrapSourcePlaced(Identity));
		}
		public GarmentScrapSource(GarmentScrapSourceReadModel readModel) : base(readModel)
		{
			Code = readModel.Code;
			Name = readModel.Name;
			Description = readModel.Description;
		}

        public void Modify()
        {
            MarkModified();
        }

        public void setCode(string code)
        {
            if (this.Code != code)
            {
                this.Code = code;
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
            if (this.Description != desc)
            {
                this.Description = desc;
                ReadModel.Description = desc;
            }
        }
    }
}