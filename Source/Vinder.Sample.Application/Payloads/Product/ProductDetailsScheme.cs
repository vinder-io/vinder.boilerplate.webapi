namespace Vinder.Sample.Application.Payloads.Product;

public sealed record ProductDetailsScheme
{
    public string Identifier { get; init; } = default!;
    public string Title { get; init; } = default!;
    public decimal Price { get; init; } = default!;
}