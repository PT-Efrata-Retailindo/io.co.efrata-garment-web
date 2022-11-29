using Infrastructure.Domain.Events;

namespace Manufactures.Domain.Events
{
    public interface IGarmentSewingInEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IGarmentSewingInEvent
    {
    }
}