using MediatR;

namespace Infrastructure.Domain.Events
{
    public interface IDomainEvent : INotification
    {
    }
}