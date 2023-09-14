using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpModels.Responses;
using OnlineShopFrontend.Components;

namespace OnlineShopFrontend.Pages
{
    public partial class AccountPage
    {
        [Inject]
        private NavigationManager Navigator { get; set; } = null!;

        [Inject]
        private IDialogService DialogService { get; set; } = null!;

        private AccountResponse? _account;
        private CancellationTokenSource _cts = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (ShopClient.IsAuthorizationTokenSet == false) return;
            _account = await ShopClient.GetCurrentAccount(_cts.Token);
        }

        public async Task LogOut()
        {
            var dialog = DialogService.Show<LogOutDialog>();
            var result = await dialog.Result;
            if (!result.Canceled)
            { 
                ShopClient.ResetAuthorizationToken();
                await LocalStorage.RemoveItemAsync("token");
                State.IsTokenChecked = false;

                Navigator.NavigateTo("/login");
            }

        }
    }
}
