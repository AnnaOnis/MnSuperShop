using OnlineShop.Domain.Entyties;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task Register(string name, string email, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(password, nameof(password));

            var existedAccount = await _accountRepository.FindAccountByEmail(email, cancellationToken);
            if (existedAccount is not null)
            {
                throw new EmailAlreadyExistsException(message: "Account with given email is already exists.");
            }
            Account account = new Account(Guid.Empty, name, email, EncryptPassword(password));
            await _accountRepository.Add(account, cancellationToken);
        }

        private static string EncryptPassword(string password)
        {
            return password;
        }
    }
}
