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
    [Route("[controller]/getone")]
    public async Task<IActionResult> GetAsync()
    {
        var products = await _productRepository.GetAsync();
        return Ok(products);
    }

    
    [HttpGet]
    public async Task<IActionResult> GetOneAsync()
    {
        var product = await _productRepository.GetOneAsync();
        return Ok(product);
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
    public async Task<IActionResult> Post( Product product)
    {
        try
        {
            if (product == null)
            {
                _logger.LogError("product object sent from client is null.");
                return BadRequest("product object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid product object sent from client.");
                return BadRequest("Invalid model object");
            }
            var res =  await _productRepository.AddAsync(product);
            if(res == Guid.Empty)
            {
                _logger.LogError($"Something went wrong inside product add  repository");
                return StatusCode(500, "Internal server error");
            }
            _logger.LogInformation($"The {product.Name} has been added with the ID:{product.Id}.");
            return Ok();
        }
        catch(Exception ex)
        {
            _logger.LogError($"Something went wrong inside product add action: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromForm] Product product)
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
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var res =  await _productRepository.DeleteAsync(id);
            if(res == 0)
                return NotFound();
            _logger.LogInformation($"The product with ID: {id} has been deleted.");
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}