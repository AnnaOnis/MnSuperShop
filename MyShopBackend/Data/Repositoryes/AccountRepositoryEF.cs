using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entyties;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.WebApi.Data.Repositoryes
{
    public class AccountRepositoryEF : EfRepository<Account>, IAccountRepository
    {
        public AccountRepositoryEF(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken)
        {
            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return Entities.SingleAsync(e => e.Email == email, cancellationToken);
        }

        public Task<Account?> FindAccountByEmail(string email, CancellationToken cancellationToken)
        {
            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return Entities.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        }
    }
}
