namespace Vinder.Sample.Domain.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    public Task<IReadOnlyCollection<Product>> GetProductsAsync(
        ProductFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountAsync(
        ProductFilters filters,
        CancellationToken cancellation = default
    );
}