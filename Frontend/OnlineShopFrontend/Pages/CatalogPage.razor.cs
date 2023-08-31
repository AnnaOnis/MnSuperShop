
using Microsoft.AspNetCore.Components;
using OnlineShop.HttpApiCient;

namespace OnlineShopFrontend.Pages
{
    public partial class CatalogPage : IDisposable
    {
        [Inject]
        private NavigationManager Navigator { get; set; } = null!;
        [Inject]
        private IOnlineShopClient MyShopClient { get; set; } = null!;
        private Product[]? _products;
        private CancellationTokenSource _cts = new();

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1), _cts.Token);
            _products = await MyShopClient.GetProducts(_cts.Token);
        }

        private void OpenProduct(Product product)
        {
            Navigator.NavigateTo($"/catalog/product/{product.Id}");
        }

        public void Dispose()
        {
            _cts?.Cancel();
        }
    }
}
