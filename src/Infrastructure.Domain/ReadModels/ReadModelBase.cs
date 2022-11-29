using ExtCore.Data.Entities.Abstractions;
using Moonlay.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Domain.ReadModels
{
    public abstract class ReadModelBase : ReadModel, IEntity, IAuditTrail, ISoftDelete
    {
        protected ReadModelBase(Guid identity)
        {
            Identity = identity;
        }

        #region IAuditTrail ISoftDelete

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public bool? Deleted { get; set; }

        public DateTimeOffset? DeletedDate { get; set; }

        public string DeletedBy { get; set; }

        #endregion IAuditTrail ISoftDelete
    }
}