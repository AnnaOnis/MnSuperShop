using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiCient;
using OnlineShop.HttpModels.Requests;

namespace OnlineShopFrontend.Pages
{
    public partial class LoginPage
    {
        [Inject]
        private ISnackbar Snackbar { get; set; } = null!;
        [Inject]
        private IOnlineShopClient MyShopClient { get; set; } = null!;
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
                await MyShopClient.Login(new LoginRequest()
                {
                    Email = model.Email,
                    Password = model.Password
                }, _cts.Token);

                Snackbar.Configuration.ShowCloseIcon = true;
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add("Выполнен вход в аккаунт", Severity.Success);
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
