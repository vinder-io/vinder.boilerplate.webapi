namespace Vinder.Sample.Infrastructure.Pipelines;

public static class ProductFiltersPipelineDefinition
{
    public static PipelineDefinition<Product, BsonDocument> FilterProducts(
        this PipelineDefinition<Product, BsonDocument> pipeline, ProductFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Product.Identifier, filters.Id),
            FilterDefinitions.MatchIfNotEmptyContains(Documents.Product.Title, filters.Title),
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}