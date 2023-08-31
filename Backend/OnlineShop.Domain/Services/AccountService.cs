using Microsoft.Extensions.Logging;
using OnlineShop.Domain.Entyties;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using System.Collections.Generic;

namespace OnlineShop.Domain.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAppPasswordHasher _hasher;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IAccountRepository accountRepository, 
            IAppPasswordHasher hasher,
            ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _hasher = hasher ?? throw new ArgumentNullException();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Register(string name, string email, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(password, nameof(password));

            var existedAccount = await _accountRepository.FindAccountByEmail(email, cancellationToken);
            if (existedAccount is not null)
            {
                throw new EmailAlreadyExistsException(message: "Аккаунт с таким email уже зарегистрирован!");
            }
            Account account = new Account(Guid.Empty, name, email, EncryptPassword(password));
            await _accountRepository.Add(account, cancellationToken);
        }

        private string EncryptPassword(string password)
        {
            string hashedPassword = _hasher.HashPassword(password);
            _logger.LogDebug(message: "Password hashed: {HashedPassword}", hashedPassword);
            return hashedPassword;
        }

        public async Task<Account> Login(string email, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(password, nameof(password));

            var account = await _accountRepository.FindAccountByEmail(email, cancellationToken);
            if (account is null)
            {
                throw new AccountNotFoundException("Account with given email not found");
            }

            var isPasswordValid = _hasher.VerifyHashedPassword(account.HashedPassword, password, out var rehashNeeded);
            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Invalid password");
            }

            if(rehashNeeded)
            {
                await RehashPasword(password, account, cancellationToken);
            }
            return account;
        }

        private Task RehashPasword(string password, Account account, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(password, nameof(password));
            ArgumentNullException.ThrowIfNull(account, nameof(account));

            account.HashedPassword = EncryptPassword(password);
            return _accountRepository.Update(account, cancellationToken);
        }
    }
}
