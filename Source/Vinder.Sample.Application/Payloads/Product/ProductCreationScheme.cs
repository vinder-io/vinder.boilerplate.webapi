namespace Vinder.Sample.Application.Payloads.Product;

public sealed record ProductCreationScheme :
    IRequest<Result<ProductDetailsScheme>>
{
    public string Title { get; init; } = default!;
    public decimal Price { get; init; } = default!;
}