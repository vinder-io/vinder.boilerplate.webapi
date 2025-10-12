namespace Vinder.Sample.Application.Handlers.Traceability;

public sealed class FetchActivitiesHandler(IActivityRepository repository) :
    IMessageHandler<ActivityFetchParameters, Result<PaginationScheme<ActivityDetailsScheme>>>
{
    public async Task<Result<PaginationScheme<ActivityDetailsScheme>>> HandleAsync(
        ActivityFetchParameters message, CancellationToken cancellation = default)
    {
        var filters = ActivityFilters.WithSpecifications()
            .WithAction(message.Action)
            .WithUser(message.UserId)
            .WithTenant(message.TenantId)
            .WithResource(message.Resource)
            .WithPagination(message.Pagination)
            .Build();

        var activities = await repository.GetActivitiesAsync(filters, cancellation: cancellation);
        var total = await repository.CountAsync(filters, cancellation: cancellation);

        var pagination = new PaginationScheme<ActivityDetailsScheme>
        {
            Items = [.. activities.Select(activity => ActivityMapper.AsResponse(activity))],
            Total = (int)total,
            PageNumber = message.Pagination.PageNumber,
            PageSize = message.Pagination.PageSize,
        };

        return Result<PaginationScheme<ActivityDetailsScheme>>.Success(pagination);
    }
}