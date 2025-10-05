namespace Vinder.Sample.Domain.Errors;

public static class ProductErrors
{
    public static readonly Error ProductDoesNotExist = new(
        Code: "#SAMPLE-ERROR-PRODUCT-81CD3",
        Description: "The product does not exist."
    );

    public static readonly Error ProductAlreadyExists = new(
        Code: "#SAMPLE-ERROR-PRODUCT-BB974",
        Description: "A product with the same title already exists."
    );
}