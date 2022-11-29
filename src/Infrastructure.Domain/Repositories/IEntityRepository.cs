using ExtCore.Data.Abstractions;
using System.Threading.Tasks;

namespace Infrastructure.Domain.Repositories
{
    public interface IEntityRepository<TEntity> : IRepository
    {
        Task Update(TEntity entity);
    }
}