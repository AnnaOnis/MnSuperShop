using Microsoft.AspNetCore.Mvc;
using MyShopBackend.Data;
using MyShopBackend.Data.Repositoryes;

namespace MyShopBackend.Controllers
{
    [Route("catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        public CatalogController(IRepository<Product> repository) 
        {
            _repository = repository;
        }

        [HttpGet("get_products")]
        public async Task<IResult> GetProducts(
                    [FromServices] IRepository<Product> repository,
                    CancellationToken cancellationToken)
        {
            try
            {
                var products = await repository.GetAll(cancellationToken);
                return Results.Ok(products);
            }
            catch (ArgumentNullException)
            {
                return Results.NotFound();
            }
            catch (OperationCanceledException)
            {
                return Results.NoContent();
            }

        }

        [HttpGet("get_product")]
        public async Task<IResult> GetProductById(
                    [FromQuery] Guid id,
                    [FromServices] IRepository<Product> repository,
                    CancellationToken cancellationToken)
        {
            try
            {
                var product = await repository.GetById(id, cancellationToken);
                return Results.Ok(product);
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        }

        [HttpPost("add_product")]
        public async Task<IResult> AddProduct(
                    [FromBody] Product product,
                    [FromServices] IRepository<Product> repository,
                    CancellationToken cancellationToken)
        {
            try
            {
                await repository.Add(product, cancellationToken);
                return Results.Ok();
            }
            catch (ArgumentNullException)
            {
                return Results.NotFound();
            }
        }

        [HttpPost("remove_product")]
        public async Task<IResult> RemoveProduct(
                            [FromQuery] Guid id,
                            [FromServices] IRepository<Product> repository,
                            CancellationToken cancellationToken)
        {
            try
            {
                await repository.Delete(id, cancellationToken);
                return Results.Ok();
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        }

        [HttpPost("update_product")]
        public async Task<IResult> UpdateProduct([FromBody] Product updatedProduct,
                                         [FromServices] IRepository<Product> repository,
                                         CancellationToken cancellationToken)
        {
            try
            {
                await repository.Update(updatedProduct, cancellationToken);
                return Results.Ok();
            }
            catch (ArgumentNullException)
            {
                return Results.NotFound();
            }

        }

    }
}
