using Api.Features.Orders.GetById; // para reutilizar o DTO OrderResponse
using MediatR;

namespace Api.Features.Orders.List;

public sealed record ListOrdersQuery(
    int Page = 1,
    int PageSize = 10,
    string? CustomerName = null,
    string? Code = null,
    string? SortBy = "createdAt", // createdAt | code | customerName | totalAmount
    string? SortDir = "desc"      // asc | desc
) : IRequest<PagedResult<OrderResponse>>;
