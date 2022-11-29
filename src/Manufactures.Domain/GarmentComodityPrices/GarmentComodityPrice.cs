using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentComodityPrices
{
    public class GarmentComodityPrice : AggregateRoot<GarmentComodityPrice, GarmentComodityPriceReadModel>
    {
        public bool IsValid { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public decimal Price { get; private set; }

        public GarmentComodityPrice(Guid identity, bool isValid, DateTimeOffset date, UnitDepartmentId unitId, string unitCode, string unitName, GarmentComodityId comodityId, string comodityCode, string comodityName, decimal price) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);

            //MarkTransient();

            Identity = identity;
            IsValid = isValid;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            Date = date;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            Price = price;

            ReadModel = new GarmentComodityPriceReadModel(Identity)
            {
                IsValid = IsValid,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                Date = Date,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                Price = Price
        };

            ReadModel.AddDomainEvent(new OnGarmentComodityPricePlaced(Identity));
        }

        public GarmentComodityPrice(GarmentComodityPriceReadModel readModel) : base(readModel)
        {
            IsValid = readModel.IsValid;
            UnitId = new UnitDepartmentId(readModel.UnitId); ;
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            Date = readModel.Date;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            Price = readModel.Price;
        }

        public void setPrice(decimal Price)
        {
            if (this.Price != Price)
            {
                this.Price = Price;
                ReadModel.Price = Price;
            }
        }

        public void setDate(DateTimeOffset Date)
        {
            if (this.Date != Date)
            {
                this.Date = Date;
                ReadModel.Date = Date;
            }
        }

        public void setValid(bool IsValid)
        {
            if (this.IsValid != IsValid)
            {
                this.IsValid = IsValid;
                ReadModel.IsValid = IsValid;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentComodityPrice GetEntity()
        {
            return this;
        }
    }
}
