using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiCient;
using OnlineShop.HttpModels.Requests;

namespace OnlineShopFrontend.Pages
{
    public partial class RegistrationPage
    {
        [Inject]
        private NavigationManager Navigator { get; set; } = null!;

        [Inject]
        private ISnackbar Snackbar { get; set; } = null!;

        private CancellationTokenSource _cts = new();
        RegisterRequest model = new RegisterRequest();
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
                var registerResponse = await ShopClient.RegistrateAccountAsync(new RegisterRequest()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmedPassword = model.ConfirmedPassword,
                }, _cts.Token);

                await LocalStorage.SetItemAsync("token", registerResponse.Token);

                Snackbar.Configuration.ShowCloseIcon = true;
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add("Вы успешно зарегистрированы", Severity.Success);

                Navigator.NavigateTo("/account");
            }
            catch(MyShopApiException e)
            {
                Snackbar.Configuration.ShowCloseIcon = true;
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add(e.Message, Severity.Error);
            }
            finally
            {
                _registrationInProgres = false;
                model.Name = "";
                model.Email = "";
                model.Password = "";
                model.ConfirmedPassword = "";
                StateHasChanged();
            }
        }
    }
}
