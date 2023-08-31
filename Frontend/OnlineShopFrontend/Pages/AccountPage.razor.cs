using Microsoft.AspNetCore.Components;
using OnlineShop.HttpApiCient;

namespace OnlineShopFrontend.Pages
{
    public partial class ProductPage
    {
        [Parameter]
        public Guid ProductId { get; set; }

        [Inject]
        private IOnlineShopClient MyShopClient { get; set; } = null!;

        private Product? _product;
        private CancellationToken _cancellationToken;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _product = await MyShopClient.GetProduct(ProductId, _cancellationToken);
        }
    }
}
