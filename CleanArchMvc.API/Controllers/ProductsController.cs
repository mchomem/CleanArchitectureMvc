namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
            => _productService = productService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var products = await _productService.GetProducts();

            if (products == null)
                return NotFound("Products not found");

            return Ok(products);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Post([FromBody] ProductDTO product)
        {
            if (product == null)
                return BadRequest();

            await _productService.Add(product);

            return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<ActionResult<ProductDTO>> Put(int id, [FromBody] ProductDTO product)
        {
            if (product == null || id != product.Id)
                return BadRequest("Invalid data");

            await _productService.Update(product);

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
                return NotFound("Product not found");

            await _productService.Remove(id);

            return Ok(product);
        }
    }
}
