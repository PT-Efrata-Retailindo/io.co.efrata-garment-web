using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Domain.Repositories
{
    public interface IAggregateRepository<TAggregate, TReadModel> : IEntityRepository<TAggregate>
    {
        IQueryable<TReadModel> Query { get; }

        List<TAggregate> Find(Expression<Func<TReadModel, bool>> condition);

        List<TAggregate> Find(IQueryable<TReadModel> readModels);
    }
}