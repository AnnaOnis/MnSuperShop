namespace MyShopBackend.Data.Repositoryes
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken);
    }
}
