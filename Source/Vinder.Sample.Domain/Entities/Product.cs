namespace Vinder.Sample.Domain.Entities;

public sealed class Product : Entity
{
    public string Title { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}