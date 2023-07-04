using MnSuperShop.Models;

namespace MnSuperShop.Pages
{
    public partial class CatalogPage
    {
        private List<Product>? products;

        protected override void OnInitialized()
        {
            products = Catalog.GetProducts();
        }

        private void OpenProduct(Product product)
        {
            Navigator.NavigateTo($"/catalog/{product.Id}");
        }
    }
}
