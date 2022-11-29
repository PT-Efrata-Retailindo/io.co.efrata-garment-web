using Infrastructure.Domain.Events;

namespace Manufactures.Domain.Events
{
    public interface IGarmentAvalProductEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IGarmentAvalProductEvent
    {
    }
}