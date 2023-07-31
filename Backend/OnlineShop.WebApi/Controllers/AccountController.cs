using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.WebApi.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
                    RegisterRequest request,
                    CancellationToken cancellationToken)
        {
            try
            {
                await _accountService.Register(request.Name, request.Email, request.Password, cancellationToken);
                return Ok();
            }
            catch (EmailAlreadyExistsException)
            {
                return Conflict(new ErrorResponse("Account with given email is already exists."));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
