using OnlineShop.Domain.Entyties;

namespace OnlineShop.WebApi.Sevices
{
    public interface ITokenService
    {
        string GenerateToken(Account account);
    }
}