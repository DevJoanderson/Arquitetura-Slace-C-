namespace Api.Features.Orders.List;

public sealed record PagedResult<T>(
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    IReadOnlyList<T> Items
);
