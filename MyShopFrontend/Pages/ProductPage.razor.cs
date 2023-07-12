using Microsoft.AspNetCore.Components;
using MyShopFrontend.Models;

namespace MyShopFrontend.Pages
{
    public partial class ProductPage
    {
        [Parameter]
        public Guid ProductId { get; set; }

        private Product? _product;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _product = Catalog.GetProductById(ProductId);
        }
    }
}
