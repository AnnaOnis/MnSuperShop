using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Interfaces
{
    public interface IAppPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyHashedPassword(string hashPassword, string providePassword, out bool rehashNeeded);

    }
}
