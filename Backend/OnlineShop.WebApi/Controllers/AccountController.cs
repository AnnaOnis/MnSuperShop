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
                return Conflict(new ErrorResponse("Аккаунт с таким email уже зарегистрирован!"));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(
                     LoginRequest request,
                     CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountService.Login(request.Email, request.Password, cancellationToken);
                return new LoginResponse(account.Id, account.Name);
            }
            catch (AccountNotFoundException) 
            { 
                return Conflict(new ErrorResponse("Аккаунтс таким Email не найден!"));
            }
            catch (InvalidPasswordException)
            {
                return Conflict(new ErrorResponse("Неверный пароль!"));
            }
        }
    }
}
