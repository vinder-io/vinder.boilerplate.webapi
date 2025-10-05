namespace Vinder.Sample.Application.Handlers.Product;

public sealed class FetchProductsHandler(IProductRepository productRepository) :
    IRequestHandler<ProductFetchParameters, Result<IReadOnlyCollection<ProductDetailsScheme>>>
{
    public async Task<Result<IReadOnlyCollection<ProductDetailsScheme>>> Handle(
        ProductFetchParameters request, CancellationToken cancellationToken)
    {
        var filters = ProductFilters.WithSpecifications()
            .WithTitle(request.Title)
            .WithPrice(request.Price)
            .WithPagination(request.Pagination)
            .WithSort(request.Sort)
            .Build();

        var products = await productRepository.GetProductsAsync(filters, cancellation: cancellationToken);
        var response = products
            .Select(product => ProductMapper.AsResponse(product))
            .ToList();

        return Result<IReadOnlyCollection<ProductDetailsScheme>>.Success(response);
    }
}