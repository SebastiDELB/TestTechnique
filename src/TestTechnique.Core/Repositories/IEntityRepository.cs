namespace TestTechnique.Core.Repositories;

public interface IEntityRepository<T> where T : class
{
    public Task<IEnumerable<T>> GetAsync();
    public Task<T> GetAsync(Guid id);
    public Task<T> GetAsync(Guid id, bool asTracking);
    public Task<Guid> AddAsync(T entity);
    public Task<IEnumerable<Guid>> AddAsync(IEnumerable<T> entities);
    public Task UpdateAsync(T entity);
    public Task UpdateAsync(IEnumerable<T> entities);
    public Task DeleteAsync(Guid id);
    public Task DeleteAsync(IEnumerable<T> entities);
}