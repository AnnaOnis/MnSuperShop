using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;
using System.Net;
using System.Net.Http.Json;

namespace OnlineShop.HttpApiCient
{
    public class OnlineShopClient : IDisposable, IOnlineShopClient
    {
        private readonly string _host;
        private readonly HttpClient _httpClient;

        public OnlineShopClient(string host = "http://myshop.com/", HttpClient? httpClient = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(host, nameof(host));

            if (!Uri.TryCreate(host, UriKind.Absolute, out var hostUri))
            {
                throw new ArgumentException("The host address should be a valid url", nameof(host));
            }
            _host = host;
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress ??= hostUri;
        }

        public void Dispose()
        {
            ((IDisposable)_httpClient).Dispose();
        }

        public async Task<Product[]> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _httpClient.GetFromJsonAsync<Product[]>("/catalog/get_products", cancellationToken);
            if (products == null)
            {
                throw new InvalidOperationException("The server returned the null products!");
            }
            else
            {
                return products;
            }
        }

        public async Task<Product> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            var product = await _httpClient.GetFromJsonAsync<Product>($"/catalog/get_product?id={id}", cancellationToken);
            if (product == null)
            {
                throw new InvalidOperationException("The server returned the null product!");
            }
            else
            {
                return product;
            }
        }

        public async Task AddProduct(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));

            using var response = await _httpClient.PostAsJsonAsync("/catalog/add_product", product, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveProduct(Guid id, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.PostAsync($"/catalog/remove_product?id={id}", null, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProduct(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));

            using var response = await _httpClient.PostAsJsonAsync("/catalog/update_product", product, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task RegistrateAccountAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            using var response = await _httpClient.PostAsJsonAsync("/account/register", request, cancellationToken);
            if(!response.IsSuccessStatusCode)
            {
                if(response.StatusCode == HttpStatusCode.Conflict)
                {
                    var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    throw new MyShopApiException(error!);
                }else if(response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var details = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    throw new MyShopApiException(response.StatusCode, details!);
                }
                else
                {
                    throw new MyShopApiException("Неизвестная ошибка!");
                }
            }
        }

        public async Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            using var response = await _httpClient.PostAsJsonAsync("/account/login", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
                    throw new MyShopApiException(error!);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var details = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken: cancellationToken);
                    throw new MyShopApiException(response.StatusCode, details!);
                }
                else
                {
                    throw new MyShopApiException($"Неизвестная ошибка!: {response.StatusCode}");
                }
            }
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken);
            return loginResponse;
        }
    }
}