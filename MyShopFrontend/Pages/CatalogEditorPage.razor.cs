using Microsoft.AspNetCore.Components;

namespace MyShopFrontend.Pages
{
    public partial class CatalogEditorPage : IDisposable
    {
        [Inject]
        private IMyShopClient MyShopClient { get; set; } = null!;
        private Product[]? _products;
        private string searchString1 = "";
        private CancellationTokenSource _cts = new();
        private Product? _selectedProduct;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _products = await MyShopClient.GetProducts(_cts.Token);
        }

        private async Task Add()
        {
          
        }
        private async Task Remove()
        {

        }
        private bool FilterFunc1(Product product) => FilterFunc(product, searchString1);

        private bool FilterFunc(Product product, string searchString)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (product.Name is not null &&  product.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (product.Description is not null &&  product.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (product.DescriptionDiscount is not null &&  product.DescriptionDiscount.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{product.Price} {product.DiscountPrice} {product.Stock} {product.ProducedAt} {product.ExpiredAt} {product.ImageUrl}".Contains(searchString))
                return true;
            return false;
        }

        public void Dispose()
        {
            _cts?.Cancel();
        }
    }
}
