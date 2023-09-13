using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;
using OnlineShop.WebApi.Sevices;
using System.Security.Claims;

namespace OnlineShop.WebApi.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(AccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(
                    RegisterRequest request,
                    CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountService.Register(request.Name, request.Email, request.Password, cancellationToken);
                var token = _tokenService.GenerateToken(account);
                return new RegisterResponse(account.Id, account.Name, account.Email, token);
            }
            catch (EmailAlreadyExistsException)
            {
                return Conflict(new ErrorResponse("Аккаунт с таким email уже зарегистрирован!"));
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
                var token = _tokenService.GenerateToken(account);
                return new LoginResponse(account.Id, account.Name, account.Email, token);
            }
            catch (AccountNotFoundException) 
            { 
                return Conflict(new ErrorResponse("Аккаунт с таким Email не найден!"));
            }
            catch (InvalidPasswordException)
            {
                return Conflict(new ErrorResponse("Неверный пароль!"));
            }
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<ActionResult<AccountResponse>> GetCurrentAccount(CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            var account = await _accountService.GetAccountById(guid, cancellationToken);
            return new AccountResponse(account.Id, account.Name, account.Email);
            //ProtectedLocalStorage
        }
    }
}
