using ExtCore.Data.EntityFramework;
using ExtCore.Data.EntityFramework.SqlServer;
using Infrastructure;
using Infrastructure.Domain.ReadModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moonlay.Domain;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DanLiris.Admin.Web
{
    public class AppStorageContext : StorageContext
    {
        private readonly IWebApiContext _workContext;
        private readonly IMediator _mediator;

        public AppStorageContext(IOptions<StorageContextOptions> options, IWebApiContext workContext, IMediator mediator) : base(options)
        {
            _workContext = workContext;
            _mediator = mediator;
        }

        public override int SaveChanges()
        {
            this.AuditTrack(_workContext);

            this.DispatchDomainEventsAsync(_mediator).Wait();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.AuditTrack(_workContext);

            await this.DispatchDomainEventsAsync(_mediator);

            return await base.SaveChangesAsync(cancellationToken);
        }
    }

    internal static class EventDispatcherExtension
    {
        public static void AuditTrack(this AppStorageContext ctx, IWebApiContext workContext)
        {
            if (workContext == null) return;

            var now = DateTime.Now;

            var addedAuditedEntities = ctx.ChangeTracker.Entries<ReadModelBase>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

            var modifiedAuditedEntities = ctx.ChangeTracker.Entries<ReadModelBase>()
              .Where(p => p.State == EntityState.Modified)
              .Select(p => p.Entity);

            if (!modifiedAuditedEntities.Any() && !addedAuditedEntities.Any())
                return;

            var currentUser = workContext.UserName ?? "System";

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedBy = currentUser;
                added.CreatedDate = now;
                added.Deleted = false;
                added.ModifiedBy = currentUser;
                added.ModifiedDate = now;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.ModifiedBy = currentUser;
                modified.ModifiedDate = now;

                if (modified is ISoftDelete)
                {
                    if (modified.Deleted.HasValue && modified.Deleted.Value)
                    {
                        modified.DeletedBy = currentUser;
                        modified.DeletedDate = now;
                    }
                }
            }
        }

        public static async Task DispatchDomainEventsAsync(this AppStorageContext ctx, IMediator mediator)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<ReadModelBase>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async d => await mediator.Publish(d));

            await Task.WhenAll(tasks);
        }
    }
}