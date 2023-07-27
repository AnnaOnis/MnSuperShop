using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpModels.Requests;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopFrontend.Pages
{
    public class RegisterAccountForm
    {
        [Required]
        [StringLength(30, ErrorMessage = "Имя должно содержать больше 3 символов!", MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Пароль не должен быть меньше 8 символов!", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string Password2 { get; set; }
    }
    public partial class RegistrationPage
    {
        [Inject]
        private ISnackbar Snackbar { get; set; } = null!;
        [Inject]
        private IOnlineShopClient MyShopClient { get; set; } = null!;
        private CancellationTokenSource _cts = new();
        RegisterAccountForm model = new RegisterAccountForm();
        bool _registrationInProgres;

        private async Task OnValidSubmit()
        {
            if (_registrationInProgres)
            {
                Snackbar.Add("Пожалуйста, подождите...", Severity.Info);
                return;
            }
            _registrationInProgres = true;
            try
            {
                await Task.Delay(1000);
                await MyShopClient.RegistrateAccountAsync(new RegisterRequest()
                {
                    Name = model.Username,
                    Email = model.Email,
                    Password = model.Password
                }, _cts.Token);

                Snackbar.Configuration.ShowCloseIcon = true;
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add("Вы успешно зарегистрированы", Severity.Success);
            }
            finally
            {
                _registrationInProgres = false;
                model.Username = "";
                model.Email = "";
                model.Password = "";
                model.Password2 = "";
                StateHasChanged();
            }
        }
    }
}
