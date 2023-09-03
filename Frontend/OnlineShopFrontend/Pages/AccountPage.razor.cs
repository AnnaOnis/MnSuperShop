using Microsoft.AspNetCore.Components;
using OnlineShop.HttpApiCient;
using OnlineShop.HttpApiCient.Models;

namespace OnlineShopFrontend.Pages
{
    public partial class AccountPage
    {
        [Inject]
        private IOnlineShopClient MyShopClient { get; set; } = null!;

        private Account? _account;
        private CancellationToken _cancellationToken;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
        }
    }
}
