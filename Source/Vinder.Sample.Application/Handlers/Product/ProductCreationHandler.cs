namespace Vinder.Sample.Application.Handlers.Product;

public sealed class ProductCreationHandler(IProductRepository productRepository, IActivityRepository activityRepository) :
    IRequestHandler<ProductCreationScheme, Result<ProductDetailsScheme>>
{
    public async Task<Result<ProductDetailsScheme>> Handle(
        ProductCreationScheme request, CancellationToken cancellationToken)
    {
        var filters = ProductFilters.WithSpecifications()
            .WithTitle(request.Title)
            .Build();

        var matchingProducts = await productRepository.GetProductsAsync(filters, cancellation: cancellationToken);
        var existingProduct = matchingProducts.FirstOrDefault();

        if (existingProduct is not null)
        {
            // raise error: #SAMPLE-ERROR-PRODUCT-BB974 (tracking purposes)
            return Result<ProductDetailsScheme>.Failure(ProductErrors.ProductAlreadyExists);
        }

        var product = ProductMapper.AsProduct(request);
        var instance = await productRepository.InsertAsync(product, cancellation: cancellationToken);

        var activity = new Activity
        {
            Action = "product.creation",
            Description = $"Product {instance.Title} ({instance.Id}) created.",
            Resource = Resource.From(instance.Id, nameof(Product)),
            Metadata = new Dictionary<string, string>
            {
                { "product.identifier", instance.Id },
            }
        };

        await activityRepository.InsertAsync(activity, cancellation: cancellationToken);

        return Result<ProductDetailsScheme>.Success(ProductMapper.AsResponse(instance));
    }
}