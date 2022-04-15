using Microsoft.AspNetCore.Mvc;
using TestTechnique.Core.Models;
using TestTechnique.Core.Repositories;

namespace TestTechnique.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductRepository _productRepository;

    public ProductController(ILogger<ProductController> logger, IProductRepository productRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var products = _productRepository.GetAsync();
        return Ok(products);
    }
    
    [HttpGet("{id:guid}")]
    public IActionResult Get([FromRoute] Guid id)
    {
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromQuery] Product product)
    {
        await _productRepository.AddAsync(product);
        return NoContent();
        _logger.LogInformation($"The {product.Name} has been added with the ID:{product.Id}.");
    }
    
    [HttpPut("{id:guid}")]
    public IActionResult Put([FromHeader] Guid id, [FromRoute] Product product)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromHeader] Guid id)
    {
        var product = new Product { Id = id };
        await _productRepository.DeleteAsync(product);
        Console.Write("The product with ID: {0} has been deleted.", id);
        return NotFound();
    }
}