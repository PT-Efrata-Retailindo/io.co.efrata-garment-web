using Infrastructure.Domain.Events;

namespace Manufactures.Domain.Events
{
    public interface IGarmentAvalProductItemEventHandler<Tevent> : IDomainEventHandler<Tevent> where Tevent : IGarmentAvalProductItemEvent
    {
    }
}