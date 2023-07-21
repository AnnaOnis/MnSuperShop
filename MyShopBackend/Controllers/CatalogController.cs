using Microsoft.AspNetCore.Mvc;
using MyShopBackend.Data;
using MyShopBackend.Data.Repositoryes;

namespace MyShopBackend.Controllers
{
    [Route("catalog")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        public CatalogController(IRepository<Product> repository) 
        {
            _repository = repository;
        }

        [HttpGet("get_products")]
        public async Task<ActionResult<Product[]>> GetProducts(
                    [FromServices] IRepository<Product> repository,
                    CancellationToken cancellationToken)
        {
            try
            {
                var products = await repository.GetAll(cancellationToken);
                return Ok(products);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpGet("get_product")]
        public async Task<ActionResult<Product>> GetProductById(
                    [FromQuery] Guid id,
                    [FromServices] IRepository<Product> repository,
                    CancellationToken cancellationToken)
        {
            try
            {
                var product = await repository.GetById(id, cancellationToken);
                return product;
            }
            catch (InvalidOperationException)
            {
                return BadRequest(id);
            }
        }

        [HttpPost("add_product")]
        public async Task<IActionResult> AddProduct(
                    Product product,
                    [FromServices] IRepository<Product> repository,
                    CancellationToken cancellationToken)
        {
            try
            {
                await repository.Add(product, cancellationToken);
                return Ok();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpPost("remove_product")]
        public async Task<IActionResult> RemoveProduct(
                            [FromQuery] Guid id,
                            [FromServices] IRepository<Product> repository,
                            CancellationToken cancellationToken)
        {
            try
            {
                await repository.Delete(id, cancellationToken);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return BadRequest(id);
            }
        }

        [HttpPost("update_product")]
        public async Task<IActionResult> UpdateProduct(
                                         Product updatedProduct,
                                         [FromServices] IRepository<Product> repository,
                                         CancellationToken cancellationToken)
        {
            try
            {
                await repository.Update(updatedProduct, cancellationToken);
                return Ok();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }

    }
}
