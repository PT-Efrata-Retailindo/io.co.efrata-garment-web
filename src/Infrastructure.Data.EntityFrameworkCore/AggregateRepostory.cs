using ExtCore.Data.EntityFramework;
using Infrastructure.Domain;
using Infrastructure.Domain.ReadModels;
using Infrastructure.Domain.Repositories;
using Moonlay;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFrameworkCore
{
    public abstract class AggregateRepostory<TAggregate, TReadModel> : RepositoryBase<TReadModel>, IAggregateRepository<TAggregate, TReadModel>
        where TAggregate : AggregateRoot<TAggregate, TReadModel>
        where TReadModel : ReadModelBase
    {
        public IQueryable<TReadModel> Query => dbSet;

        public virtual List<TAggregate> Find(IQueryable<TReadModel> readModels)
        {
            return readModels.Select(o => Map(o)).ToList();
        }

        public virtual List<TAggregate> Find(Expression<Func<TReadModel, bool>> condition)
        {
            return Query.Where(condition).Select(o => Map(o)).ToList();
        }

        protected abstract TAggregate Map(TReadModel readModel);

        public virtual Task Update(TAggregate aggregate)
        {
            if (aggregate.IsTransient())
                dbSet.Add(aggregate.GetReadModel());
            else if (aggregate.IsModified())
                dbSet.Update(aggregate.GetReadModel());
            else if (aggregate.IsRemoved())
            {
                var readModel = aggregate.GetReadModel();
                if (readModel is ISoftDelete)
                {
                    readModel.Deleted = true;
                    dbSet.Update(readModel);
                }
                else
                    dbSet.Remove(readModel);
            }

            return Task.CompletedTask;
        }
    }
}