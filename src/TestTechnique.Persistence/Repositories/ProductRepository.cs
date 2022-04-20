using Microsoft.EntityFrameworkCore;
using TestTechnique.Core.Models;
using TestTechnique.Core.Repositories;

namespace TestTechnique.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly TestTechniqueDbContext _dbContext;

    public ProductRepository(TestTechniqueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAsync()
    {

        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product> GetAsync(Guid id)
    {
        var res = await _dbContext.Products.FindAsync(id);
        if(res == null)
            return new Product();
        return res;
    }

    public Task<Product> GetAsync(Guid id, bool asTracking)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> AddAsync(Product product)
    {

        try
        {
            var result = _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product.Id;
        }
        catch
        {
            return Guid.Empty;
        }
    }

    public async Task<IEnumerable<Guid>> AddAsync(IEnumerable<Product> products)
    {
        List<Guid> ids = new List<Guid>();
        try
        {
            foreach (var product in products)
            {
                await _dbContext.Products.AddAsync(product);
                ids.Add(product.Id);
            }
            await _dbContext.SaveChangesAsync();
            return ids;
        }
        catch
        {
            return ids;
        }
       
    }

    public async Task UpdateAsync(Product product)
    {
        var result = await _dbContext.Products
               .FirstOrDefaultAsync(p => p.Id == product.Id);

        if (result != null)
        {
            result.Name = product.Name;
            result.Description = product.Description;
            result.Price = product.Price;

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(IEnumerable<Product> products)
    {
        foreach(var product in products)
        {
            var result = await _dbContext.Products
              .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (result != null)
            {
                result.Name = product.Name;
                result.Description = product.Description;
                result.Price = product.Price;

                await _dbContext.SaveChangesAsync();
            }
        }
    }

    public async Task DeleteAsync(Product product)
    {
        var result = await _dbContext.Products
                 .FirstOrDefaultAsync(p => p.Id == product.Id);
        if (result != null)
        {
            _dbContext.Products.Remove(result);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(IEnumerable<Product> products)
    {
        foreach(Product product in products)
        {
            var result = await _dbContext.Products
                 .FirstOrDefaultAsync(p => p.Id == product.Id);
            if (result != null)
            {
                _dbContext.Products.Remove(result);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

    public async Task<Product> GetByNameAsync(string name)
    {
        var result = await _dbContext.Products
                  .FirstOrDefaultAsync(p => p.Name == name);
        if(result != null) 
            return result;

        return null;
    }

    Task<Guid> IEntityRepository<Product>.AddAsync(Product entity)
    {
        throw new NotImplementedException();
    }
}