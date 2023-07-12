using MyShopFrontend.Models;
using MyShopFrontend.Services.Interfaces;
using System.Net.Http.Json;

namespace MyShopFrontend.Services
{
    public class MyShopClient : IDisposable, IMyShopClient
    {
        private readonly string _host;
        private readonly HttpClient _httpClient;

        public MyShopClient(string host = "http://myshop.com/", HttpClient? httpClient = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(host, nameof(host));
            if (!Uri.TryCreate(host, UriKind.Absolute, out var hostUri))
            {
                throw new ArgumentException("The host address should be a valid url", nameof(host));
            }
            _host = host;
            _httpClient = httpClient ?? new HttpClient();
            if (_httpClient.BaseAddress is null)
            {
                _httpClient.BaseAddress = hostUri;
            }
        }

        public void Dispose()
        {
            ((IDisposable)_httpClient).Dispose();
        }

        public async Task<Product[]> GetProducts()
        {
            var products = await _httpClient.GetFromJsonAsync<Product[]>("/get_products");
            if (products == null)
            {
                throw new InvalidOperationException("The server returned the null products!");
            }
            return products;
        }

        public async Task<Product> GetProduct(Guid id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            var product = await _httpClient.GetFromJsonAsync<Product>($"/get_product?id={id}");
            if (product == null)
            {
                throw new InvalidOperationException("The server returned the null product!");
            }
            return product;
        }

        public async Task AddProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));

            using var response = await _httpClient.PostAsJsonAsync("/get_products", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveProduct(Guid id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            using var response = await _httpClient.PostAsJsonAsync($"/remove_product", id);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));

            using var response = await _httpClient.PostAsJsonAsync("/update_product_fb", product);
            response.EnsureSuccessStatusCode();
        }
    }
}
