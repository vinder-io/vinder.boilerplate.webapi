namespace Vinder.Sample.Domain.Filtering.Builders;

public sealed class ProductFiltersBuilder :
    FiltersBuilderBase<ProductFilters, ProductFiltersBuilder>
{
    public ProductFiltersBuilder WithTitle(string? title)
    {
        _filters.Title = title;
        return this;
    }

    public ProductFiltersBuilder WithPrice(decimal? price)
    {
        _filters.Price = price;
        return this;
    }
}