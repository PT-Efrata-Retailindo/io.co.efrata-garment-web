using Infrastructure.Domain.Events;
namespace Manufactures.Domain.Events
{
    public interface IGarmentSewingInItemEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IGarmentSewingInItemEvent
    {
    }
}