using Microsoft.AspNetCore.Components;
using MnSuperShop.Models;

namespace MnSuperShop.Pages
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
