using Microsoft.Extensions.Logging;
using OnlineShop.Domain.Entyties;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OnlineShop.Domain.Services
{
    public class AccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppPasswordHasher _hasher;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IUnitOfWork uow, 
            IAppPasswordHasher hasher,
            ILogger<AccountService> logger)
        {
            _unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
            _hasher = hasher ?? throw new ArgumentNullException();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Account> Register(string name, string email, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(password, nameof(password));

            var existedAccount = await _unitOfWork.AccountRepository.FindAccountByEmail(email, cancellationToken);
            if (existedAccount is not null)
            {
                throw new EmailAlreadyExistsException(message: "Аккаунт с таким email уже зарегистрирован!");
            }
            Account account = new Account(Guid.Empty, name, email, EncryptPassword(password));
            Cart cart = new Cart(Guid.Empty, account.Id);

            await _unitOfWork.AccountRepository.Add(account, cancellationToken);
            await _unitOfWork.CartRepository.Add(cart, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return account;
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

            var account = await _unitOfWork.AccountRepository.FindAccountByEmail(email, cancellationToken);
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
            return _unitOfWork.AccountRepository.Update(account, cancellationToken);
        }

        public Task<Account> GetAccountById(Guid id, CancellationToken cancellationToken)
        {
            return _unitOfWork.AccountRepository.GetById(id, cancellationToken);
        }
    }
}
