using Microsoft.AspNetCore.Mvc;
using MyShopBackend.Data;
using MyShopBackend.Data.Repositoryes;

namespace MyShopBackend.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRepository<Account> _repository;

        public AccountController(IRepository<Account> repository)
        {
            _repository = repository;
        }

        [HttpPost("register") ]
        public async Task<IActionResult> AddAccunt(
                    Account account,
                    [FromServices] IRepository<Account> repository,
                    CancellationToken cancellationToken)
        {
            try
            {
                await repository.Add(account, cancellationToken);
                return Ok();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
