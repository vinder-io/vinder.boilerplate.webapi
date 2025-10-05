namespace Vinder.Sample.WebApi.Controllers;

[ApiController]
[Route("api/v1/products")]
public sealed class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProductAsync(
        [FromQuery] ProductFetchParameters request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),
        };
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(
        ProductCreationScheme request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status201Created, result.Data),

            // raise error: #SAMPLE-ERROR-PRODUCT-BB974 (tracking purposes)
            { IsFailure: true } when result.Error == ProductErrors.ProductAlreadyExists =>
                StatusCode(StatusCodes.Status409Conflict, result.Error)
        };
    }
}