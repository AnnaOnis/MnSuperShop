
using Microsoft.AspNetCore.Identity;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.IdentityPasswordHasher
{
    public class IdentityPasswordHasher : IAppPasswordHasher
    {
        private readonly PasswordHasher<object> _passwordHasher = new();
        private readonly object _fake = new();
        public string HashPassword(string password)
        {
            ArgumentNullException.ThrowIfNull(nameof(password));
            return _passwordHasher.HashPassword(_fake, password);
        }

        public bool VerifyHashedPassword(string hashPassword, string providePassword, out bool rehashNeeded)
        {
            ArgumentNullException.ThrowIfNull(nameof(hashPassword));
            ArgumentNullException.ThrowIfNull(nameof(providePassword));

            var result = _passwordHasher.VerifyHashedPassword(_fake, hashPassword, providePassword);
            rehashNeeded = result == PasswordVerificationResult.SuccessRehashNeeded;
            return result != PasswordVerificationResult.Failed;
        }
    }
}
