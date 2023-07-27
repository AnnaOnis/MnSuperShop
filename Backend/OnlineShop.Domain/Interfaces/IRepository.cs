using OnlineShop.Domain.Entyties;

namespace OnlineShop.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> GetById(Guid Id, CancellationToken cancellationToken);
        Task<IReadOnlyList<TEntity>> GetAll(CancellationToken cancellationToken);
        Task Add(TEntity entity, CancellationToken cancellationToken);
        Task Update(TEntity entity, CancellationToken cancellationToken);
        Task Delete(Guid Id, CancellationToken cancellationToken);
    }
}
