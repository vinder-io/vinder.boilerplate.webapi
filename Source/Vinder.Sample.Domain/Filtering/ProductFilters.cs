namespace Vinder.Sample.Domain.Filtering;

public sealed class ProductFilters : Filters
{
    public string? Title { get; set; }
    public decimal? Price { get; set; }

    public static ProductFiltersBuilder WithSpecifications() => new();
}