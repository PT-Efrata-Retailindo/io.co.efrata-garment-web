using Infrastructure.Domain.Events;
using Infrastructure.Domain.ReadModels;
using Moonlay.Domain;
using System;
using System.Linq;

namespace Infrastructure.Domain
{
    public abstract class EntityBase<TEntity> : ReadModelBase
    {
        protected EntityBase(Guid identity) : base(identity)
        {
            this.MarkTransient();

            this.AddDomainEvent(new OnEntityCreated<TEntity>(GetEntity()));
        }

        public IAuditTrail AuditTrail => this;

        public ISoftDelete SoftDelete => this;

        protected override void MarkModified()
        {
            if (!this.IsTransient())
            {
                Moonlay.Validator.ThrowWhenTrue(() => IsRemoved(), "Entity cannot be modified, it was set as Deleted Entity");

                base.MarkModified();
                if (this.DomainEvents == null || !this.DomainEvents.Any(o => o is OnEntityUpdated<TEntity>))
                    this.AddDomainEvent(new OnEntityUpdated<TEntity>(GetEntity()));
            }
        }

        protected abstract TEntity GetEntity();

        protected override void MarkRemoved()
        {
            if (!this.IsTransient())
            {
                base.MarkRemoved();

                if (this.DomainEvents == null || !this.DomainEvents.Any(o => o is OnEntityDeleted<TEntity>))
                    this.AddDomainEvent(new OnEntityDeleted<TEntity>(GetEntity()));

                // clear updated events
                if (this.DomainEvents.Any(o => o is OnEntityUpdated<TEntity>))
                {
                    this.DomainEvents.Where(o => o is OnEntityUpdated<TEntity>)
                        .ToList()
                        .ForEach(o => this.RemoveDomainEvent(o));
                }
            }
        }
    }
}