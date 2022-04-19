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
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var products = await _productRepository.GetAsync();
        return Ok(products);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var product = await _productRepository.GetAsync(id);
        if(product == null)
            return NotFound();
        return Ok(product);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] Product product)
    {
        var res = await _productRepository.AddAsync(product);
        if(res == Guid.Empty)
            return BadRequest();
        _logger.LogInformation($"The {product.Name} has been added with the ID:{product.Id}.");
        return Ok(res);
        
    }
    
    [HttpPut]
    public async Task<IActionResult> Put([FromForm] Product product)
    {
        try
        {
            await _productRepository.UpdateAsync(product);
            _logger.LogInformation($"The {product.Name} has been updated");
            return Ok();
        }
        catch
        {
            return NotFound();
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete( [FromForm] Product product)
    {
        try
        {
            await _productRepository.DeleteAsync(product);
            _logger.LogInformation($"The product with ID: {product.Id} has been deleted.");
            return Ok();
        }
        catch
        {
            return NotFound();
        }

        
       
    }
}