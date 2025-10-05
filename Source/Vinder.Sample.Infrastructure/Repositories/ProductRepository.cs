namespace Vinder.Sample.Infrastructure.Repositories;

public sealed class ProductRepository(IMongoDatabase database) :
    BaseRepository<Product>(database, Collections.Products),
    IProductRepository
{
    public async Task<IReadOnlyCollection<Product>> GetProductsAsync(
        ProductFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Product>()
            .As<Product, Product, BsonDocument>()
            .FilterProducts(filters)
            .Paginate(filters.Pagination)
            .Sort(filters.Sort);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var products = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Product>(bson))
            .ToList();

        return products;
    }

    public async Task<long> CountAsync(ProductFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Product>()
            .As<Product, Product, BsonDocument>()
            .FilterProducts(filters)
            .Count();

        var aggregation = await _collection.AggregateAsync(pipeline, cancellationToken: cancellation);
        var result = await aggregation.FirstOrDefaultAsync(cancellation);

        return result?.Count ?? 0;
    }
}