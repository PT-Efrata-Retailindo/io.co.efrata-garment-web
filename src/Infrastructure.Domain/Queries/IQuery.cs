using MediatR;

namespace Infrastructure.Domain.Queries
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }

    public interface IQueryHandler<TComand, TResponse> : IRequestHandler<TComand, TResponse> where TComand : IQuery<TResponse>
    {
    }
}