using TestTechnique.Core.Models;

namespace TestTechnique.Core.Repositories;

public interface IProductRepository : IEntityRepository<Product>
{
    public Task<Product> GetByNameAsync(string name);

}