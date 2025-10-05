namespace Vinder.Sample.Application.Payloads.Product;

public sealed record ProductFetchParameters :
    IRequest<Result<IReadOnlyCollection<ProductDetailsScheme>>>
{
    public string? Identifier { get; init; }
    public string? Title { get; init; }
    public decimal? Price { get; init; }

    public SortFilters? Sort { get; init; }
    public PaginationFilters? Pagination { get; init; }
}