using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TestTechnique.Api.Controllers;
using TestTechnique.Core.Models;
using TestTechnique.Core.Repositories;
using Xunit;

namespace TestTechnique.Api.Test.Controllers;

public class ProductControllerTest
{
    private readonly ProductController _productController;
    private readonly Mock<IProductRepository> _productRepository;

    public ProductControllerTest()
    {
        _productRepository = new Mock<IProductRepository>();
        _productController = new ProductController(new NullLogger<ProductController>(), _productRepository.Object);
    }

    [Fact]
    public async Task Get_Many()
    {
        // Arrange
        _productRepository
            .Setup(x => x.GetAsync())
            .ReturnsAsync(new List<Product>());
        
        // Act
        var response = await _productController.Get();

        // Assert
        Assert.NotNull(response);
        var content = Assert.IsAssignableFrom<OkObjectResult>(response);
        Assert.NotNull(content.Value); 
        Assert.IsAssignableFrom<IEnumerable<Product>>(content.Value);
    }
    
    [Fact]
    public async Task Get_One()
    {
        // Arrange
        _productRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Product());
        
        // Act
        var response = await _productController.GetAsync(Guid.NewGuid());

        // Assert
        Assert.NotNull(response);
        var content = Assert.IsAssignableFrom<OkObjectResult>(response);
        Assert.NotNull(content.Value); 
        Assert.IsType<Product>(content.Value);
    }
    
    [Fact]
    public async Task Get_One_NotFound()
    {
        // Arrange
        _productRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product) null);
        
        // Act
        var response = await _productController.GetAsync(Guid.NewGuid());

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<NotFoundResult>(response);
    }
    
    [Fact]
    public async Task Post()
    {
        // Arrange
        _productRepository
            .Setup(x => x.AddAsync(It.IsAny<Product>()));

        Product product1 = new Product(){
            Id = Guid.NewGuid(),
            Name = "name",
            Description = "description",
            Price = 5
        };
        // Act
        var response = await _productController.Post(product1);

        // Assert
      //  Assert.NotNull(response);
        var content = Assert.IsAssignableFrom<CreatedAtActionResult>(response);
       // Assert.NotNull(content.Value);
        //Assert.Equal("Post", content.ActionName);
    }
    
    [Fact]
    public async Task Put()
    {
        // Arrange
        _productRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Product());
        _productRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Product>()));

        // Act
        var response = await _productController.Put(Guid.NewGuid() ,new Product()) ;

        // Assert
        Assert.NotNull(response);
        var content = Assert.IsAssignableFrom<OkObjectResult>(response);
        Assert.NotNull(content.Value);
        Assert.IsAssignableFrom<Product>(content.Value);
    }
    
    [Fact]
    public async Task Put_NotFound()
    {
        // Arrange
        _productRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product) null);
        _productRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Product>()));
        
        // Act
        var response = await _productController.Put(Guid.NewGuid(),new Product());

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<NotFoundResult>(response);
    }
    
    [Fact]
    public async Task Delete()
    {
        // Arrange
        _productRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Product());
        _productRepository
            .Setup(x => x.DeleteAsync(It.IsAny<Product>()));
        
        // Act
        var response = await _productController.Delete( new Product());

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<NoContentResult>(response);
    }
    
    [Fact]
    public async Task Delete_NotFound()
    {
        // Arrange
        _productRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product) null);
        _productRepository
            .Setup(x => x.DeleteAsync(It.IsAny<Product>()));
        
        // Act
        var response = await _productController.Delete(new Product());

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<NotFoundResult>(response);
    }
}