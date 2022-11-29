using Infrastructure.Domain.Events;

namespace Manufactures.Domain.Events
{
    public interface IGarmentPreparingEventHandler<TEvent>:IDomainEventHandler<TEvent> where TEvent : IGarmentPreparingEvent
    {
    }
}