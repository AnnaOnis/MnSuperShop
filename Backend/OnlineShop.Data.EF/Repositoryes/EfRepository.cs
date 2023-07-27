using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entyties;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Data.EF.Repositoryes
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly AppDbContext _dbContext;
        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();


        public virtual async Task<IReadOnlyList<TEntity>> GetAll(CancellationToken cancellationToken)
            => await Entities.ToListAsync(cancellationToken);
        public virtual async Task<TEntity> GetById(Guid Id, CancellationToken cancellationToken)
            => await Entities.FirstAsync(x => x.Id == Id, cancellationToken);
        public virtual async Task Add(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Entities.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public virtual async Task Update(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public virtual async Task Delete(Guid Id, CancellationToken cancellationToken)
        {
            var entity = await Entities.FirstAsync(x => x.Id == Id, cancellationToken);
            Entities.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
