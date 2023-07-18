
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

        public async Task<Product[]> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _httpClient.GetFromJsonAsync<Product[]>("/get_products", cancellationToken);
            if (products == null)
            {
                throw new InvalidOperationException("The server returned the null products!");
            }
            return products;
        }

        public async Task<Product> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            ArgumentException.

            var product = await _httpClient.GetFromJsonAsync<Product>($"/get_product?id={id}", cancellationToken);
            if (product == null)
            {
                throw new InvalidOperationException("The server returned the null product!");
            }
            return product;
        }

        public async Task AddProduct(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));

            using var response = await _httpClient.PostAsJsonAsync("/get_products", product, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveProduct(Guid id, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            using var response = await _httpClient.PostAsJsonAsync($"/remove_product", id, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProduct(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));

            using var response = await _httpClient.PostAsJsonAsync("/update_product_fb", product, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }

