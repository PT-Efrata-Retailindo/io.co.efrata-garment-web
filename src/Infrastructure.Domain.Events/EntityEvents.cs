namespace Infrastructure.Domain.Events
{
    public class OnEntityCreated<T> : IDomainEvent
    {
        public OnEntityCreated(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }

    public class OnEntityUpdated<T> : IDomainEvent
    {
        public OnEntityUpdated(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }

    public class OnEntityDeleted<T> : IDomainEvent
    {
        public OnEntityDeleted(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}