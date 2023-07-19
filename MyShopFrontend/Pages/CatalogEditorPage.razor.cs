using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyShopFrontend.Components;

namespace MyShopFrontend.Pages
{
    public partial class CatalogEditorPage : IDisposable
    {
        [Inject]
        private IDialogService DialogService { get; set; } = null!;
        [Inject]
        private IMyShopClient MyShopClient { get; set; } = null!;
        private List<Product> _products = new();
        private string searchString1 = "";
        private CancellationTokenSource _cts = new();
        private Product? _selectedProduct;
        private Product? _selectedProductBeforeEdit;
        private Product? _addedOrUpdatedProduct;
        private Product? _deletedProduct;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _products = (await MyShopClient.GetProducts(_cts.Token)).ToList();
        }

        private void AddItemToTable()
        {
            _addedOrUpdatedProduct = new Product("Новый товар", 0);
            _products.Add(_addedOrUpdatedProduct);
        }

        private async Task RemoveItem(object element)
        {
            var deletedProduct = (Product)element;
            var dialog = DialogService.Show<DeleteDialog>("Подтвердить удаление");
            var result = await dialog.Result;
            if (!result.Canceled)
            {
               _products.Remove(deletedProduct);               
                StateHasChanged();
                await MyShopClient.RemoveProduct(deletedProduct.Id, _cts.Token);
            }
        }

        private void SelectProduct(object element)
        {
            _selectedProduct = (Product)element;
        }
        private void BackupItem(object element)
        {
            _selectedProductBeforeEdit = (Product)element;
        }

        private void ResetItemToOriginalValues(object element)
        {

        }

        private async void AddOrUpdateProductInDB(object element)
        {
            _addedOrUpdatedProduct = (Product)element;
            var products = await MyShopClient.GetProducts(_cts.Token);
            if (_addedOrUpdatedProduct is not null && !Array.Exists(products, p=>p.Id == _addedOrUpdatedProduct.Id))
            {
                await MyShopClient.AddProduct(_addedOrUpdatedProduct, _cts.Token);
            }
            else if(_addedOrUpdatedProduct is not null && Array.Exists(products, p => p.Id == _addedOrUpdatedProduct.Id))
            {
                await MyShopClient.UpdateProduct(_addedOrUpdatedProduct, _cts.Token);
            }
        }
        private bool FilterFunc1(Product product) => FilterFunc(product, searchString1);

        private bool FilterFunc(Product product, string searchString)
        {

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
