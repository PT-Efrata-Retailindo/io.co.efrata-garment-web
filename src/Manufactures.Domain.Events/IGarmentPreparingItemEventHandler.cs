using Infrastructure.Domain.Events;
namespace Manufactures.Domain.Events
{
    public interface IGarmentPreparingItemEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IGarmentPreparingItemEvent
    {
    }
}