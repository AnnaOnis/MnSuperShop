using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiCient;
using OnlineShop.HttpModels.Responses;

namespace OnlineShopFrontend.Pages
{
    public partial class AccountPage
    {
        [Inject]
        private ISnackbar Snackbar { get; set; } = null!;

        private AccountResponse? _account;
        private CancellationTokenSource _cts = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                _account = await ShopClient.GetCurrentAccount(_cts.Token);
            }
            catch(MyShopApiException e)
            {
                Snackbar.Configuration.ShowCloseIcon = true;
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add(e.Message, Severity.Error);
            }
        }

        public void LogOut()
        {
            ShopClient.ResetAuthorizationToken();
            _account = null!;
            State.IsTokenChecked = false;
        }
    }
}
