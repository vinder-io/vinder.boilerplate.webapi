namespace Vinder.Sample.Application.Handlers.Traceability;

public sealed class FetchActivitiesHandler(IActivityRepository repository) :
    IRequestHandler<ActivityFetchParameters, Result<PaginationScheme<ActivityDetailsScheme>>>
{
    public async Task<Result<PaginationScheme<ActivityDetailsScheme>>> Handle(
        ActivityFetchParameters request, CancellationToken cancellationToken)
    {
        var filters = ActivityFilters.WithSpecifications()
            .WithAction(request.Action)
            .WithUser(request.UserId)
            .WithTenant(request.TenantId)
            .WithResource(request.Resource)
            .WithPagination(request.Pagination)
            .Build();

        var activities = await repository.GetActivitiesAsync(filters, cancellation: cancellationToken);
        var total = await repository.CountAsync(filters, cancellation: cancellationToken);

        var pagination = new PaginationScheme<ActivityDetailsScheme>
        {
            Items = [.. activities.Select(activity => ActivityMapper.AsResponse(activity))],
            Total = (int)total,
            PageNumber = request.Pagination.PageNumber,
            PageSize = request.Pagination.PageSize,
        };

        return Result<PaginationScheme<ActivityDetailsScheme>>.Success(pagination);
    }
}