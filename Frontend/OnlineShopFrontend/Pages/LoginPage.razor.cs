using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiCient;
using OnlineShop.HttpModels.Requests;

namespace OnlineShopFrontend.Pages
{
    public partial class LoginPage
    {
        [Inject]
        private NavigationManager Navigator { get; set; } = null!;

        [Inject]
        private ISnackbar Snackbar { get; set; } = null!;

        private CancellationTokenSource _cts = new();
        LoginRequest model = new LoginRequest();
        bool _loginInProgres;

        private async Task OnValidSubmit()
        {
            if (_loginInProgres)
            {
                Snackbar.Add("Пожалуйста, подождите...", Severity.Info);
                return;
            }
            _loginInProgres = true;
            try
            {
                await Task.Delay(1000);
                var loginResponse = await ShopClient.Login(new LoginRequest()
                {
                    Email = model.Email,
                    Password = model.Password
                }, _cts.Token);

                await LocalStorage.SetItemAsync("token", loginResponse.Token);

                Snackbar.Configuration.ShowCloseIcon = true;
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add("Выполнен вход в аккаунт", Severity.Success);

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
                model.Email = "";
                model.Password = "";
                StateHasChanged();
                _loginInProgres = false;
            }
        }
    }
}
