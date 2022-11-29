using Infrastructure.Domain.Events;

namespace Manufactures.Domain.Events
{
    public interface IGarmentSewingDOEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IGarmentSewingDOEvent
    {
    }
}