using MediatR;

namespace Infrastructure.Domain.Commands
{
    public interface ICommand<TResponse> : IRequest<TResponse>
    {
    }

    public interface ICommandHandler<TComand, TResponse> : IRequestHandler<TComand, TResponse> where TComand : ICommand<TResponse>
    {
    }
}