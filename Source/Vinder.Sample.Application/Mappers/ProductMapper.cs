namespace Vinder.Sample.Application.Mappers;

public static class ProductMapper
{
    public static Product AsProduct(ProductCreationScheme scheme) => new()
    {
        Title = scheme.Title,
        Price = scheme.Price
    };

    public static ProductDetailsScheme AsResponse(Product product) => new()
    {
        Identifier = product.Id,
        Title = product.Title,
        Price = product.Price
    };
}