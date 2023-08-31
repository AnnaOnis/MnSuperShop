using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiCient;
using OnlineShopFrontend.Components;

namespace OnlineShopFrontend.Pages
{
    public partial class CatalogEditorPage : IDisposable
    {
        [Inject]
        private IDialogService DialogService { get; set; } = null!;
        [Inject]
        private IOnlineShopClient MyShopClient { get; set; } = null!;
        private List<Product> _products = new();
        private string searchString1 = "";
        private CancellationTokenSource _cts = new();
        private Product? _selectedProduct;
        private Product? _selectedProductBeforeEdit;
        private Product? _addedOrUpdatedProduct;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _products = (await MyShopClient.GetProducts(_cts.Token)).ToList();
        }

        private void AddItemToTable()
        {
            _addedOrUpdatedProduct = new Product("А", 0);
            _products.Add(_addedOrUpdatedProduct);
        }

        private async Task RemoveItem(object element)
        {

            var product = (Product)element;
            var dialog = DialogService.Show<DeleteDialog>("Подтвердить удаление");
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                if (product == _addedOrUpdatedProduct)
                {
                    _products.Remove(product);
                    StateHasChanged();
                }
                else
                {
                    _products.Remove(product);
                    StateHasChanged();
                    await MyShopClient.RemoveProduct(product.Id, _cts.Token);
                }

            }
        }

        private void SelectProduct(object element)
        {

        }
        private void BackupItem(object element)
        {

        }

        private void ResetItemToOriginalValues(object element)
        {
            if (_selectedProductBeforeEdit is not null)
            {
                ((Product)element).Name = _selectedProductBeforeEdit.Name;
                ((Product)element).Price = _selectedProductBeforeEdit.Price;
                ((Product)element).DiscountPrice = _selectedProductBeforeEdit.DiscountPrice;
                ((Product)element).Stock = _selectedProductBeforeEdit.Stock;
                ((Product)element).Description = _selectedProductBeforeEdit.Description;
                ((Product)element).DescriptionDiscount = _selectedProductBeforeEdit.DescriptionDiscount;
                ((Product)element).ExpiredAt = _selectedProductBeforeEdit.ExpiredAt;
                ((Product)element).ImageUrl = _selectedProductBeforeEdit.ImageUrl;
                ((Product)element).ProducedAt = _selectedProductBeforeEdit.ProducedAt;
            }
        }

        private async void AddOrUpdateProductInDB(object element)
        {
            _addedOrUpdatedProduct = (Product)element;
            var products = await MyShopClient.GetProducts(_cts.Token);
            if (!Array.Exists(products, p => p.Id == _addedOrUpdatedProduct.Id))
            {
                await MyShopClient.AddProduct(_addedOrUpdatedProduct, _cts.Token);
            }
            else
            {
                await MyShopClient.UpdateProduct(_addedOrUpdatedProduct, _cts.Token);
            }
        }
        private bool FilterFunc1(Product product) => FilterFunc(product, searchString1);

        private bool FilterFunc(Product product, string searchString)
        {

            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (product.Name is not null && product.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (product.Description is not null && product.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (product.DescriptionDiscount is not null && product.DescriptionDiscount.Contains(searchString, StringComparison.OrdinalIgnoreCase))
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
