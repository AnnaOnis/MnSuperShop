using MyShopFrontend.Models;
using MyShopFrontend.Services;

namespace MyShopFrontend.Pages
{
    public partial class CatalogPage
    {
        private Product[]? products;

        protected override async Task OnInitializedAsync()
        {
            products = await MyShopClient.GetProducts();
        }

        private void OpenProduct(Product product)
        {
            Navigator.NavigateTo($"/catalog/{product.Id}");
        }
    }
}
