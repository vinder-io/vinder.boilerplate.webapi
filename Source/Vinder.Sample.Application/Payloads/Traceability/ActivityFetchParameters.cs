namespace Vinder.Sample.Application.Payloads.Traceability;

public sealed record ActivityFetchParameters :
    IRequest<Result<PaginationScheme<ActivityDetailsScheme>>>
{
    public string? Action { get; init; }
    public string? Resource { get; init; }
    public string? UserId { get; init; }
    public string? TenantId { get; init; }

    public PaginationFilters Pagination { get; init; } = new();
    public SortFilters? Sort { get; init; }
}