namespace Vinder.Sample.Application.Validators.Product;

public sealed class ProductCreationValidator : AbstractValidator<ProductCreationScheme>
{
    public ProductCreationValidator()
    {
        RuleFor(product => product.Title)
            .NotEmpty()
            .WithMessage("product title is required.")
            .MaximumLength(150)
            .WithMessage("product title cannot exceed 150 characters.");

        RuleFor(product => product.Price)
            .GreaterThan(0)
            .WithMessage("product price must be greater than zero.");
    }
}